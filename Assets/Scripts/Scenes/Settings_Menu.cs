using UnityEngine;
using UnityEngine.UI;

public class Settings_Menu : MonoBehaviour
{
    public RectTransform panel;
    public Slider volumeSlider;

    public Vector2 hiddenPos;
    public Vector2 shownPos;

    private bool isOpen = false;

    void Start()
    {
        // Load saved volume (default = 1)
        float savedVolume = PlayerPrefs.GetFloat("volume", 1f);

        // Safety clamp (prevents accidental full mute)
        savedVolume = Mathf.Clamp01(savedVolume);

        AudioListener.volume = savedVolume;
        volumeSlider.value = savedVolume;

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

    public AudioSource videoAudioSource;

    public void ChangeVolume(float value)
    {
        value = Mathf.Clamp01(value);

        AudioListener.volume = value;

        if (videoAudioSource != null)
            videoAudioSource.volume = value;

        PlayerPrefs.SetFloat("volume", value);
        PlayerPrefs.Save();
    }
}