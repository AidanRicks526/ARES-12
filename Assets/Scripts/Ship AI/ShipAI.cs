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
    public bool isManualMode = false;
    public float textSpeedMultiplier = 1f;
    public float fadeDuration = 0.3f;

    private List<VoiceLine> currentDialogue = new List<VoiceLine>();
    private int index = 0;

    private bool isPlaying;
    public bool IsPlaying => isPlaying;

    void Start()
    {
        HideUIInstant();
    }

    public void OnNextLinePressed()
    {
        // Used by UI button to advance dialogue
        // Only relevant if you expand manual mode later
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
                index++;
                continue;
            }

            speakerText.text = line.speakerName;

            yield return TypeText(line);

            float duration = line.voiceClip != null
                ? line.voiceClip.length
                : line.fallbackDuration;

            if (line.voiceClip != null)
            {
                voiceSource.clip = line.voiceClip;
                voiceSource.Play();
            }

            if (line.choices != null && line.choices.Length > 0)
            {
                yield return HandleChoices(line);
            }
            else
            {
                yield return new WaitForSeconds(duration);
                index++;
            }
        }

        yield return Fade(0f);
        isPlaying = false;
    }

    bool CanPlay(VoiceLine line)
    {
        // Inventory check (USES YOUR SYSTEM)
        if (line.requiredItem != null)
        {
            if (!Inventory.Instance.HasItem(line.requiredItem))
                return false;
        }

        // Timer check
        if (line.triggerBeforeTime > 0f)
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

        if (index < 0)
            index++;
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