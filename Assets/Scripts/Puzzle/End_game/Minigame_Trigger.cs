using UnityEngine;
using System.Collections;

public class Minigame_Trigger : MonoBehaviour
{
    [Header("Reference")]
    public GameObject imageObject; // Drag "Fade_IN" here

    private CanvasGroup imageCanvasGroup;

    void Start()
    {
        // Automatically find or add the group so you don't have to do it manually
        imageCanvasGroup = imageObject.GetComponent<CanvasGroup>();
        if (imageCanvasGroup == null)
            imageCanvasGroup = imageObject.AddComponent<CanvasGroup>();
    }

    [Header("Settings")]
    public float fadeDuration = 1f;

    private bool triggered = false;

    void Update()
    {
        if (triggered) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            triggered = true;
            StartCoroutine(FadeInThenDestroy());
        }
    }

    IEnumerator FadeInThenDestroy()
    {
        float t = 0f;

        // ensure starting invisible
        imageCanvasGroup.alpha = 0f;
        imageCanvasGroup.gameObject.SetActive(true);

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = t / fadeDuration;
            imageCanvasGroup.alpha = Mathf.Lerp(0f, 1f, normalized);
            yield return null;
        }

        imageCanvasGroup.alpha = 1f;

        // small pause so it “lands”
        yield return new WaitForSeconds(0.2f);

        // destroy THIS object (the trigger object)
        Destroy(gameObject);
    }
}