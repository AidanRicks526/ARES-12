using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Settings_Menu : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform panel;
    public Slider volumeSlider;
    public Toggle manualDialogueToggle; // Add a reference to your UI Toggle
    public Vector2 hiddenPos;
    public Vector2 shownPos;

    [Header("External References")]
    public ShipAI shipAI; // Link your ShipAI script here in the Inspector

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
        // 0 = Auto, 1 = Manual. Default to Auto (0)
        bool isManual = PlayerPrefs.GetInt("manualDialogue", 0) == 1;

        if (manualDialogueToggle != null)
            manualDialogueToggle.isOn = isManual;

        SetDialogueMode(isManual);
    }

    public void OnManualToggleChanged(bool isManual)
    {
        if (shipAI != null)
        {
            shipAI.isManualMode = isManual;

            // tell the ShipAI to act like the button was pressed to "break" the loop
            if (!isManual)
            {
                shipAI.OnNextLinePressed();
            }
        }

        PlayerPrefs.SetInt("manualDialogue", isManual ? 1 : 0);
        PlayerPrefs.Save();
    }

    void SetDialogueMode(bool isManual)
    {
        if (shipAI != null)
        {
            shipAI.isManualMode = isManual;
        }
    }

    // --- YOUR EXISTING UI & AUDIO LOGIC ---
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
                if (audio != null) audio.volume = value;
        }
    }

    void ApplyVideoVolume(float value)
    {
        if (videoPlayers != null)
        {
            foreach (var vp in videoPlayers)
                if (vp != null) { vp.SetDirectAudioMute(0, false); vp.SetDirectAudioVolume(0, value); }
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
                    while (!vp.isPrepared) yield return null;
                    ApplyVideoVolume(value);
                }
            }
        }
    }


}