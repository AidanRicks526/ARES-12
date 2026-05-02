using UnityEngine;
using System.Collections;

public class Minigame_FadeManager : MonoBehaviour
{
    public CanvasGroup failImage;
    public CanvasGroup winImage;

    public float fadeDuration = 1f;

    public void ShowFail()
    {
        StartCoroutine(FadeIn(failImage));
    }

    public void ShowWin()
    {
        StartCoroutine(FadeIn(winImage));
    }

    IEnumerator FadeIn(CanvasGroup cg)
    {
        cg.gameObject.SetActive(true);
        cg.alpha = 0f;

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        cg.alpha = 1f;
    }
}