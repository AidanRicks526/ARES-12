using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorLockedUI : MonoBehaviour
{
    public static DoorLockedUI Instance;

    [Header("UI")]
    public CanvasGroup canvasGroup;
    public Text messageText;

    [Header("Settings")]
    public float fadeDuration = 1f;
    public float displayTime = 1.5f;

    void Awake()
    {
        Instance = this;
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    public void Show(string message)
    {
        StopAllCoroutines();
        messageText.text = message;
        gameObject.SetActive(true);
        StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
        // Fade in
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = t / fadeDuration;
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // Wait
        yield return new WaitForSeconds(displayTime);

        // Fade out
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1f - (t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}