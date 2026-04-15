using UnityEngine;
using UnityEngine.UI;

public class Settings_Menu : MonoBehaviour
{
    public RectTransform panel;
    public Slider volumeSlider;

    public Vector2 hiddenPos;
    public Vector2 shownPos;

    private bool isOpen = false;

    [Header("Audio")]
    public AudioSource[] allAudioSources; // ALL game audio
    public AudioSource videoAudioSource;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("volume", 1f);
        savedVolume = Mathf.Clamp01(savedVolume);

        volumeSlider.value = savedVolume;
        ApplyVolume(savedVolume);

        panel.anchoredPosition = hiddenPos;
        isOpen = false;
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
        value = Mathf.Clamp01(value);

        ApplyVolume(value);

        PlayerPrefs.SetFloat("volume", value);
        PlayerPrefs.Save();
    }

    void ApplyVolume(float value)
    {
        // apply to ALL audio sources
        if (allAudioSources != null)
        {
            foreach (var audio in allAudioSources)
            {
                if (audio != null)
                    audio.volume = value;
            }
        }

        // ensure video audio is also controlled
        if (videoAudioSource != null)
            videoAudioSource.volume = value;
    }
}