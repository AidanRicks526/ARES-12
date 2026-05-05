using System.Collections;
using UnityEngine;

public class EndingFadeAll : MonoBehaviour
{
    public CanvasGroup[] images;

    public float fadeDuration = 2f;

    void Start()
    {
        StartCoroutine(FadeAllIn());
    }

    IEnumerator FadeAllIn()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);

            foreach (var img in images)
            {
                if (img != null)
                    img.alpha = alpha;
            }

            yield return null;
        }

        // Ensure fully visible
        foreach (var img in images)
        {
            if (img != null)
                img.alpha = 1f;
        }

        Debug.Log("All images faded in together");
    }
}