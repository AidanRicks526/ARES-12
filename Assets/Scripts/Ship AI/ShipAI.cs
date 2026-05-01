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
    public float textSpeedMultiplier = 1f;
    public float fadeDuration = 0.3f;

    [Header("Legacy Compatibility")]
    public bool isManualMode = false;

    /// <summary>
    /// Legacy hook for old UI systems.
    /// No longer used in auto dialogue flow.
    /// </summary>
    public void OnNextLinePressed()
    {
        if (!isManualMode) return;

        if (!IsPlaying) return;

        Debug.Log("Manual advance requested (legacy mode)");

        // Force skip current wait (safe advance)
        StopAllCoroutines();
    }

    private List<VoiceLine> currentDialogue;
    private int index;
    private bool isPlaying;

    public bool IsPlaying => isPlaying;

    void Start()
    {
        HideUIInstant();
    }

    public void PlayDialogue(List<VoiceLine> lines)
    {
        if (isPlaying) return;

        currentDialogue = lines;
        index = 0;

        StartCoroutine(RunDialogue());
    }

    IEnumerator RunDialogue()
    {
        isPlaying = true;
        yield return Fade(1f);

        while (index < currentDialogue.Count)
        {
            VoiceLine line = currentDialogue[index];

            if (!CanPlay(line))
            {
                index = line.nextIndex >= 0 ? line.nextIndex : index + 1;
                continue;
            }

            speakerText.text = line.speakerName;

            yield return TypeText(line);

            float duration = Mathf.Max(
                line.voiceClip != null ? line.voiceClip.length : line.fallbackDuration,
                line.subtitle.Length * line.typeSpeed
            );

            if (line.voiceClip != null)
            {
                voiceSource.clip = line.voiceClip;
                voiceSource.Play();
            }

            // BRANCHING SYSTEM
            if (line.choices != null && line.choices.Length > 0)
            {
                yield return HandleChoices(line);
                continue;
            }

            yield return new WaitForSeconds(duration);

            // LINEAR FLOW
            index = line.nextIndex >= 0 ? line.nextIndex : index + 1;
        }

        yield return Fade(0f);
        isPlaying = false;
    }

    bool CanPlay(VoiceLine line)
    {
        if (line.requiredItem != null)
        {
            if (!Inventory.Instance.HasItem(line.requiredItem))
                return false;

            if (line.consumeItem)
                Inventory.Instance.RemoveItem(line.requiredItem);
        }

        if (line.useTimerGate)
        {
            if (GameTimer.Instance != null &&
                GameTimer.Instance.currentTime > line.triggerBeforeTime)
                return false;
        }

        return true;
    }

    IEnumerator HandleChoices(VoiceLine line)
    {
        bool picked = false;
        int chosenIndex = -1;

        ChoiceUI.Instance.OnChoiceSelected = (i) =>
        {
            chosenIndex = i;
            picked = true;
        };

        ChoiceUI.Instance.Show(line.choices);

        yield return new WaitUntil(() => picked);

        index = line.choices[chosenIndex].nextIndex;
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

    IEnumerator Fade(float target)
    {
        float start = dialogueCanvasGroup.alpha;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            dialogueCanvasGroup.alpha = Mathf.Lerp(start, target, t / fadeDuration);
            yield return null;
        }

        dialogueCanvasGroup.alpha = target;
    }

    void HideUIInstant()
    {
        dialogueCanvasGroup.alpha = 0f;
    }
}