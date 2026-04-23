using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipAI_2 : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup dialogueCanvasGroup;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI subtitleText;

    [Tooltip("The button the player clicks to advance text in Manual Mode.")]
    public GameObject continueButton;

    [Header("Audio")]
    public AudioSource voiceSource;
    public AudioSource sfxSource;

    [Header("Settings")]
    public float fadeDuration = 0.3f;
    public bool isManualMode = false;

    private Queue<VoiceLine> queue = new Queue<VoiceLine>();
    private bool isPlaying = false;
    private bool nextButtonPressed = false;

    void Start()
    {
        // Start hidden
        if (dialogueCanvasGroup != null)
        {
            dialogueCanvasGroup.alpha = 0f;
            dialogueCanvasGroup.interactable = false;
            dialogueCanvasGroup.blocksRaycasts = false;
        }

        if (continueButton != null) continueButton.SetActive(false);
    }


    public void OnNextLinePressed()
    {
        nextButtonPressed = true;
    }

    public void PlayVoiceLine(VoiceLine line)
    {
        queue.Enqueue(line);
        if (!isPlaying) StartCoroutine(ProcessQueue());
    }

    IEnumerator ProcessQueue()
    {
        isPlaying = true;

        // 1. Dialogue panel appears
        yield return StartCoroutine(FadeCanvas(1f));

        while (queue.Count > 0)
        {
            VoiceLine line = queue.Dequeue();
            speakerText.text = line.speakerName;

            // 2. Typewriter text plays
            yield return StartCoroutine(TypeText(line));

            if (line.voiceClip != null)
            {
                voiceSource.clip = line.voiceClip;
                voiceSource.Play();
            }

            // 3. Handle Waiting
            if (isManualMode)
            {
                // Show the button ONLY once typing is done
                if (continueButton != null) continueButton.SetActive(true);

                nextButtonPressed = false;
                yield return new WaitUntil(() => nextButtonPressed);

                // Hide button immediately so they can't double-click it for the next line
                if (continueButton != null) continueButton.SetActive(false);
            }
            else
            {
                // Auto mode: button stays hidden, wait for duration
                if (continueButton != null) continueButton.SetActive(false);
                float duration = (line.voiceClip != null) ? line.voiceClip.length : line.fallbackDuration;
                yield return new WaitForSeconds(duration);
            }
        }

        // 4. Dialogue panel disappears (along with everything inside it)
        yield return StartCoroutine(FadeCanvas(0f));

        // Final cleanup just in case
        if (continueButton != null) continueButton.SetActive(false);

        isPlaying = false;
    }

    IEnumerator TypeText(VoiceLine line)
    {
        subtitleText.text = "";
        foreach (char c in line.subtitle)
        {
            subtitleText.text += c;
            if (line.typingSFX != null) sfxSource.PlayOneShot(line.typingSFX);
            yield return new WaitForSeconds(line.typeSpeed);
        }
    }

    IEnumerator FadeCanvas(float target)
    {
        float start = dialogueCanvasGroup.alpha;
        float time = 0f;

        // Toggle interactivity so the button can't be clicked while invisible
        dialogueCanvasGroup.interactable = (target > 0);
        dialogueCanvasGroup.blocksRaycasts = (target > 0);

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            dialogueCanvasGroup.alpha = Mathf.Lerp(start, target, time / fadeDuration);
            yield return null;
        }

        dialogueCanvasGroup.alpha = target;
    }
}