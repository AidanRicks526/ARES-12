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

    [Header("Fade Target")]
    public GameObject fadeObject;

    [Header("UI")]
    public GameObject instructionsPanel;

    [Header("Ghost Player")]
    public GameObject extraObjectToActivate;

    public float fadeDuration = 1f;

    private CanvasGroup canvasGroup;

    private bool triggered;
    private bool playerInside;

    void Start()
    {
        if (fadeObject == null)
        {
            Debug.LogError("[Minigame] fadeObject NOT assigned");
            return;
        }

        canvasGroup = fadeObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = fadeObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        fadeObject.SetActive(false);

        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);

        Debug.Log("[Minigame] Ready");
    }

    void Update()
    {
        if (triggered) return;

        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("[Minigame] E pressed inside trigger");
            ShowInstructions();
        }
    }

    void ShowInstructions()
    {
        if (instructionsPanel == null)
        {
            Debug.LogWarning("[Minigame] instructionsPanel missing");
            return;
        }

        instructionsPanel.SetActive(true);
        Debug.Log("[Minigame] Instructions shown");
    }

    // ✔ BUTTON CALLS THIS DIRECTLY (NO HIDDEN STATE)
    public void StartMinigame()
    {
        Debug.Log("[Minigame] Button clicked");

        if (triggered)
        {
            Debug.Log("[Minigame] Already triggered");
            return;
        }

        if (!playerInside)
        {
            Debug.Log("[Minigame] Player not inside trigger");
            return;
        }

        triggered = true;

        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);

        StartCoroutine(StartMinigameSequence());
    }

    IEnumerator StartMinigameSequence()
    {
        Debug.Log("[Minigame] Starting sequence");

        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null && obj != gameObject)
                obj.SetActive(false);
        }

        if (mazeObject != null) mazeObject.SetActive(true);
        if (playerObject != null) playerObject.SetActive(true);
        if (extraObjectToActivate != null) extraObjectToActivate.SetActive(true);

        fadeObject.SetActive(true);

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(0.2f);

        Debug.Log("[Minigame] Sequence complete (no destroy)");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("[Minigame] Player entered trigger");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            instructionsPanel?.SetActive(false);
            Debug.Log("[Minigame] Player exited trigger");
        }
    }
}