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

    [Header("Text Speed")]
    public float textSpeed = 1f;

    [Header("Settings")]
    public float fadeDuration = 0.3f;

    private List<VoiceLine> currentDialogue;
    private int index;
    private bool isPlaying;

    public bool IsPlaying => isPlaying;

    void Start()
    {
        HideUIInstant();
    }

    // =========================
    // TEXT SPEED
    // =========================
    public void SetTextSpeed(float value)
    {
        textSpeed = Mathf.Max(0.1f, value);
    }

    // =========================
    // START DIALOGUE
    // =========================
    public void PlayDialogue(List<VoiceLine> lines)
    {
        if (isPlaying) return;

        currentDialogue = lines;

        // 🔥 IMPORTANT: choose correct entry point BEFORE starting
        index = GetStartIndex();

        StartCoroutine(RunDialogue());
    }

    // =========================
    // ENTRY RESOLUTION (NEW)
    // =========================
    int GetStartIndex()
    {
        for (int i = 0; i < currentDialogue.Count; i++)
        {
            if (CanPlay(currentDialogue[i]))
                return i;
        }

        return 0;
    }

    // =========================
    // CONDITION CHECK
    // =========================
    bool CanPlay(VoiceLine line)
    {
        if (line.requiredItem != null)
        {
            if (!Inventory.Instance.HasItem(line.requiredItem))
                return false;

            if (line.consumeItem)
                Inventory.Instance.RemoveItem(line.requiredItem);
        }

        return true;
    }

    // =========================
    // MAIN LOOP
    // =========================
    IEnumerator RunDialogue()
    {
        isPlaying = true;
        yield return Fade(1f);

        while (index >= 0 && index < currentDialogue.Count)
        {
            VoiceLine line = currentDialogue[index];

            // 🔥 Skip invalid lines safely
            if (!CanPlay(line))
            {
                index = line.nextIndex >= 0 ? line.nextIndex : index + 1;
                continue;
            }

            speakerText.text = line.speakerName;

            // =========================
            // PLAY VOICE
            // =========================
            if (line.voiceClip != null)
            {
                voiceSource.clip = line.voiceClip;
                voiceSource.Play();
            }

            // =========================
            // TYPE TEXT
            // =========================
            yield return TypeText(line);

            // =========================
            // CHOICES
            // =========================
            if (line.choices != null && line.choices.Length > 0)
            {
                yield return HandleChoices(line);
                continue;
            }

            // =========================
            // NEXT BUTTON FLOW
            // =========================
            bool nextPressed = false;

            ChoiceUI.Instance.OnNextPressed = () =>
            {
                if (voiceSource != null && voiceSource.isPlaying)
                    voiceSource.Stop();

                nextPressed = true;
            };

            ChoiceUI.Instance.Show(null);

            // wait for voice to finish
            while (voiceSource != null && voiceSource.isPlaying)
                yield return null;

            // wait for player input
            yield return new WaitUntil(() => nextPressed);

            index = line.nextIndex >= 0 ? line.nextIndex : index + 1;
        }

        yield return Fade(0f);
        isPlaying = false;
    }

    // =========================
    // TYPE TEXT
    // =========================
    IEnumerator TypeText(VoiceLine line)
    {
        subtitleText.text = "";

        foreach (char c in line.subtitle)
        {
            subtitleText.text += c;

            if (line.typingSFX != null)
                sfxSource.PlayOneShot(line.typingSFX);

            yield return new WaitForSeconds(0.05f / textSpeed);
        }
    }

    // =========================
    // CHOICES
    // =========================
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

    // =========================
    // FADE
    // =========================
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

    // =========================
    // FORCE EXIT
    // =========================
    public void OnNextLinePressed()
    {
        if (!isPlaying) return;

        StopAllCoroutines();
        isPlaying = false;
        dialogueCanvasGroup.alpha = 0f;
    }
}