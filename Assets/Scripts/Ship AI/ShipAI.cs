using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipAI : MonoBehaviour
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

    [Range(0.1f, 5f)]
    public float textSpeedMultiplier = 1f;

    private Queue<VoiceLine> queue = new Queue<VoiceLine>();
    private bool isPlaying = false;
    private bool nextButtonPressed = false;

    public bool IsPlaying => isPlaying;

    void Start()
    {
        if (dialogueCanvasGroup != null)
        {
            dialogueCanvasGroup.alpha = 0f;
            dialogueCanvasGroup.interactable = false;
            dialogueCanvasGroup.blocksRaycasts = false;
        }

        if (continueButton != null)
            continueButton.SetActive(false);
    }

    public void OnNextLinePressed()
    {
        nextButtonPressed = true;
    }

    public void PlayVoiceLine(VoiceLine line)
    {
        queue.Enqueue(line);
        if (!isPlaying)
            StartCoroutine(ProcessQueue());
    }

    IEnumerator ProcessQueue()
    {
        isPlaying = true;

        yield return StartCoroutine(FadeCanvas(1f));

        while (queue.Count > 0)
        {
            VoiceLine line = queue.Dequeue();
            speakerText.text = line.speakerName;

            yield return StartCoroutine(TypeText(line));

            float duration = line.fallbackDuration;

            if (line.voiceClip != null)
            {
                voiceSource.clip = line.voiceClip;
                voiceSource.Play();
                duration = line.voiceClip.length;
            }

            if (isManualMode)
            {
                if (continueButton != null)
                    continueButton.SetActive(true);

                nextButtonPressed = false;
                yield return new WaitUntil(() => nextButtonPressed);

                if (continueButton != null)
                    continueButton.SetActive(false);
            }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           else
            {
                if (continueButton != null)
                    continueButton.SetActive(false);

                yield return new WaitForSeconds(duration);
            }
        }

        yield return StartCoroutine(FadeCanvas(0f));

        if (continueButton != null)
            continueButton.SetActive(false);

        isPlaying = false;
    }

    IEnumerator TypeText(VoiceLine line)
    {
        subtitleText.text = "";

        foreach (char c in line.subtitle)
        {
            subtitleText.text += c;

            if (line.typingSFX != null)
                sfxSource.PlayOneShot(line.typingSFX);

            yield return new WaitForSeconds(line.typeSpeed / textSpeedMultiplier);
        }
    }

    IEnumerator FadeCanvas(float target)
    {
        float start = dialogueCanvasGroup.alpha;
        float time = 0f;

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