using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Minigame_Trigger : MonoBehaviour
{
    [Header("Scene Objects")]
    public GameObject mazeObject;
    public GameObject playerObject;

    [Header("Objects to Disable On Start")]
    public List<GameObject> objectsToDisable = new List<GameObject>();

    [Header("Fade Target (ANY GameObject)")]
    public GameObject fadeObject;

    [Header("Ghost Player")]
    public GameObject extraObjectToActivate;

    public float fadeDuration = 1f;

    private CanvasGroup canvasGroup;
    private bool triggered;

    void Start()
    {
        // Ensure CanvasGroup exists
        canvasGroup = fadeObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = fadeObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        fadeObject.SetActive(false);
    }

    void Update()
    {
        if (triggered) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            triggered = true;
            StartCoroutine(StartMinigameSequence());
        }
    }

    IEnumerator StartMinigameSequence()
    {
        // 🔥 Disable everything in list
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // Step 1: activate gameplay
        mazeObject.SetActive(true);
        playerObject.SetActive(true);

        // ✨ activate extra object
        if (extraObjectToActivate != null)
            extraObjectToActivate.SetActive(true);

        // Step 2: activate fade object
        fadeObject.SetActive(true);

        // Step 3: fade in
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = t / fadeDuration;

            canvasGroup.alpha = Mathf.Lerp(0f, 1f, normalized);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(0.2f);

        Destroy(gameObject);
    }
}