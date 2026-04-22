using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipAI : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup dialogueCanvasGroup;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI subtitleText;

    [Header("Audio")]
    public AudioSource voiceSource;
    public AudioSource sfxSource;

    [Header("Settings")]
    public float fadeDuration = 0.3f;

    private Queue<VoiceLine> queue = new Queue<VoiceLine>();
    private bool isPlaying = false;

    void Start()
    {
        // Ensure UI starts hidden
        dialogueCanvasGroup.alpha = 0f;
    }

    public void PlayVoiceLine(VoiceLine line)
    {
        queue.Enqueue(line);

        if (!isPlaying)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    IEnumerator ProcessQueue()
    {
        isPlaying = true;

        // Fade in UI
        yield return StartCoroutine(FadeCanvas(1f));

        while (queue.Count > 0)
        {
            VoiceLine line = queue.Dequeue();

            speakerText.text = line.speakerName;

            // Typewriter effect
            yield return StartCoroutine(TypeText(line));

            float duration = line.fallbackDuration;

            if (line.voiceClip != null)
            {
                voiceSource.clip = line.voiceClip;
                voiceSource.Play();
                duration = line.voiceClip.length;
            }

            yield return new WaitForSeconds(duration);
        }

        // Fade out UI
        yield return StartCoroutine(FadeCanvas(0f));

        isPlaying = false;
    }

    IEnumerator TypeText(VoiceLine line)
    {
        subtitleText.text = "";

        foreach (char c in line.subtitle)
        {
            subtitleText.text += c;

            if (line.typingSFX != null)
            {
                sfxSource.PlayOneShot(line.typingSFX);
            }

            yield return new WaitForSeconds(line.typeSpeed);
        }
    }

    IEnumerator FadeCanvas(float target)
    {
        float start = dialogueCanvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            dialogueCanvasGroup.alpha = Mathf.Lerp(start, target, time / fadeDuration);
            yield return null;
        }

        dialogueCanvasGroup.alpha = target;
    }
}