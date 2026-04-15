using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Settings_Menu : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform panel;
    public Slider volumeSlider;
    public Vector2 hiddenPos;
    public Vector2 shownPos;

    [Header("Audio Settings")]
    public AudioSource[] allAudioSources;
    public VideoPlayer[] videoPlayers;

    private bool isOpen = false;

    void Start()
    {
        // 1. Setup UI state
        panel.anchoredPosition = hiddenPos;
        isOpen = false;

        // 2. Load saved volume
        float savedVolume = PlayerPrefs.GetFloat("volume", 1f);
        savedVolume = Mathf.Clamp01(savedVolume);
        volumeSlider.value = savedVolume;

        // 3. Apply to standard audio immediately
        ApplyStandardAudio(savedVolume);

        // 4. Handle VideoPlayer audio (waits for it to be ready)
        StartCoroutine(ApplyVideoVolumeDelayed(savedVolume));
    }

    // --- UI LOGIC ---
    public void ToggleMenu()
    {
        StopAllCoroutines();
        StartCoroutine(Slide(isOpen ? hiddenPos : shownPos));
        isOpen = !isOpen;
    }

    System.Collections.IEnumerator Slide(Vector2 target)
    {
        Vector2 start = panel.anchoredPosition;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 6f;
            panel.anchoredPosition = Vector2.Lerp(start, target, t);
            yield return null;
        }
        panel.anchoredPosition = target;
    }

    // --- AUDIO LOGIC ---
    public void ChangeVolume(float value)
    {
        value = Mathf.Clamp01(value);

        // Update both types of audio
        ApplyStandardAudio(value);
        ApplyVideoVolume(value);

        PlayerPrefs.SetFloat("volume", value);
        PlayerPrefs.Save();
    }

    // Helper: Standard Audio
    void ApplyStandardAudio(float value)
    {
        if (allAudioSources != null)
        {
            foreach (var audio in allAudioSources)
            {
                if (audio != null) audio.volume = value;
            }
        }
    }

    // Helper: VideoPlayer Audio (Direct)
    void ApplyVideoVolume(float value)
    {
        if (videoPlayers != null)
        {
            foreach (var vp in videoPlayers)
            {
                if (vp != null)
                {
                    vp.SetDirectAudioMute(0, false); // Force unmute
                    vp.SetDirectAudioVolume(0, value);
                }
            }
        }
    }

    // Coroutine: Waits for video to be ready before setting volume
    System.Collections.IEnumerator ApplyVideoVolumeDelayed(float value)
    {
        if (videoPlayers != null)
        {
            foreach (var vp in videoPlayers)
            {
                if (vp != null)
                {
                    while (!vp.isPrepared)
                    {
                        yield return null; // Wait for the video to prepare
                    }
                    ApplyVideoVolume(value); // Now safe to set volume
                }
            }
        }
    }
}