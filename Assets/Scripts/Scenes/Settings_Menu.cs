using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Settings_Menu : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform panel;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider textSpeedSlider;

    public Vector2 hiddenPos;
    public Vector2 shownPos;

    [Header("External References")]
    public ShipAI shipAI;

    [Header("Audio")]
    public AudioSource[] musicSources;
    public AudioSource[] sfxSources;
    public VideoPlayer[] videoPlayers;

    private bool isOpen = false;

    void Start()
    {
        panel.anchoredPosition = hiddenPos;

        // ---------------- MUSIC ----------------
        float musicVol = PlayerPrefs.GetFloat("musicVolume", 1f);
        if (musicSlider) musicSlider.value = musicVol;
        ApplyMusic(musicVol);

        // ---------------- SFX ----------------
        float sfxVol = PlayerPrefs.GetFloat("sfxVolume", 1f);
        if (sfxSlider) sfxSlider.value = sfxVol;
        ApplySFX(sfxVol);

        // ---------------- VIDEO ----------------
        StartCoroutine(ApplyVideoVolumeDelayed(musicVol));

        // ---------------- TEXT SPEED ----------------
        float textSpeed = PlayerPrefs.GetFloat("textSpeed", 1f);
        if (textSpeedSlider) textSpeedSlider.value = textSpeed;
        SetTextSpeed(textSpeed);
    }

    // =========================
    // MUSIC
    // =========================
    public void OnMusicVolumeChanged(float value)
    {
        ApplyMusic(value);
        PlayerPrefs.SetFloat("musicVolume", value);
        PlayerPrefs.Save();
    }

    void ApplyMusic(float value)
    {
        foreach (var audio in musicSources)
            if (audio != null)
                audio.volume = value;

        ApplyVideoVolume(value);
    }

    // =========================
    // SFX
    // =========================
    public void OnSFXVolumeChanged(float value)
    {
        ApplySFX(value);
        PlayerPrefs.SetFloat("sfxVolume", value);
        PlayerPrefs.Save();
    }

    void ApplySFX(float value)
    {
        foreach (var audio in sfxSources)
            if (audio != null)
                audio.volume = value;
    }

    // =========================
    // VIDEO
    // =========================
    void ApplyVideoVolume(float value)
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

    IEnumerator ApplyVideoVolumeDelayed(float value)
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

    // =========================
    // TEXT SPEED (FIXED LINK)
    // =========================
    public void OnTextSpeedChanged(float value)
    {
        SetTextSpeed(value);

        PlayerPrefs.SetFloat("textSpeed", value);
        PlayerPrefs.Save();
    }

    void SetTextSpeed(float value)
    {
        if (shipAI != null)
        {
            // 🔥 THIS is the important fix:
            shipAI.SetTextSpeed(value);
        }
    }

    // =========================
    // MENU TOGGLE
    // =========================
    public void ToggleMenu()
    {
        StopAllCoroutines();
        StartCoroutine(Slide(isOpen ? hiddenPos : shownPos));
        isOpen = !isOpen;
    }

    IEnumerator Slide(Vector2 target)
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
}