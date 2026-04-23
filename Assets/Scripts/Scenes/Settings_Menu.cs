using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Settings_Menu : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform panel;
    public Slider volumeSlider;
    public Slider textSpeedSlider; // 🔥 NEW
    public Toggle manualDialogueToggle;
    public Vector2 hiddenPos;
    public Vector2 shownPos;

    [Header("External References")]
    public ShipAI shipAI;

    [Header("Audio Settings")]
    public AudioSource[] allAudioSources;
    public VideoPlayer[] videoPlayers;

    private bool isOpen = false;

    void Start()
    {
        panel.anchoredPosition = hiddenPos;
        isOpen = false;

        // --- Load Volume ---
        float savedVolume = PlayerPrefs.GetFloat("volume", 1f);
        volumeSlider.value = savedVolume;
        ApplyStandardAudio(savedVolume);
        StartCoroutine(ApplyVideoVolumeDelayed(savedVolume));

        // --- Load Dialogue Mode ---
        bool isManual = PlayerPrefs.GetInt("manualDialogue", 0) == 1;

        if (manualDialogueToggle != null)
            manualDialogueToggle.isOn = isManual;

        SetDialogueMode(isManual);

        // --- Load Text Speed ---
        float savedTextSpeed = PlayerPrefs.GetFloat("textSpeed", 1f);

        if (textSpeedSlider != null)
            textSpeedSlider.value = savedTextSpeed;

        SetTextSpeed(savedTextSpeed);
    }

    public void OnManualToggleChanged(bool isManual)
    {
        if (shipAI != null)
        {
            shipAI.isManualMode = isManual;

            if (!isManual)
                shipAI.OnNextLinePressed();
        }

        PlayerPrefs.SetInt("manualDialogue", isManual ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnTextSpeedChanged(float value) // 🔥 NEW
    {
        SetTextSpeed(value);

        PlayerPrefs.SetFloat("textSpeed", value);
        PlayerPrefs.Save();
    }

    void SetTextSpeed(float value) // 🔥 NEW
    {
        if (shipAI != null)
            shipAI.textSpeedMultiplier = value;
    }

    void SetDialogueMode(bool isManual)
    {
        if (shipAI != null)
            shipAI.isManualMode = isManual;
    }

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

    public void ChangeVolume(float value)
    {
        ApplyStandardAudio(value);
        ApplyVideoVolume(value);
        PlayerPrefs.SetFloat("volume", value);
        PlayerPrefs.Save();
    }

    void ApplyStandardAudio(float value)
    {
        if (allAudioSources != null)
        {
            foreach (var audio in allAudioSources)
                if (audio != null)
                    audio.volume = value;
        }
    }

    void ApplyVideoVolume(float value)
    {
        if (videoPlayers != null)
        {
            foreach (var vp in videoPlayers)
            {
                if (vp != null)
                {
                    vp.SetDirectAudioMute(0, false);
                    vp.SetDirectAudioVolume(0, value);
                }
            }
        }
    }

    System.Collections.IEnumerator ApplyVideoVolumeDelayed(float value)
    {
        if (videoPlayers != null)
        {
            foreach (var vp in videoPlayers)
            {
                if (vp != null)
                {
                    while (!vp.isPrepared)
                        yield return null;

                    ApplyVideoVolume(value);
                }
            }
        }
    }
}