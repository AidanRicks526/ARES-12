using UnityEngine;
using UnityEngine.UI; // Required for RawImage
using System.Collections;

public class Minigame_fade_manager : MonoBehaviour
{
    [Header("UI Images")]
    public RawImage failImage; // Changed from Image to RawImage
    public RawImage winImage;  // Changed from Image to RawImage

    public float fadeDuration = 1f;

    public void ShowFail()
    {
        StartCoroutine(FadeIn(failImage));
    }

    public void ShowWin()
    {
        StartCoroutine(FadeIn(winImage));
    }

    IEnumerator FadeIn(RawImage img) // Changed parameter type
    {
        img.gameObject.SetActive(true);

        Color c = img.color;
        c.a = 0f;
        img.color = c;

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = t / fadeDuration;

            c.a = Mathf.Lerp(0f, 1f, normalized);
            img.color = c;

            yield return null;
        }

        c.a = 1f;
        img.color = c;
    }
}