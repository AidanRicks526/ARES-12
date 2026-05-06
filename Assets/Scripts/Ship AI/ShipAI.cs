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
    public AudioSource typingSFXSource;

    [Header("Typing")]
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
    // START DIALOGUE
    // =========================
    public void PlayDialogue(List<VoiceLine> lines)
    {
        if (isPlaying) return;

        currentDialogue = lines;
        index = GetStartIndex();

        StartCoroutine(RunDialogue());
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

            // ITEM CHECK (IMPORTANT FIX POINT)
            if (!CanPlay(line))
            {
                index = line.nextIndex >= 0 ? line.nextIndex : index + 1;
                continue;
            }

            speakerText.text = line.speakerName;

            // VOICE
            if (line.voiceClip != null && voiceSource != null)
            {
                voiceSource.clip = line.voiceClip;
                voiceSource.Play();
            }

            // TYPE TEXT (PER LETTER SOUND HERE)
            yield return TypeText(line);

            // CHOICES
            if (line.choices != null && line.choices.Length > 0)
            {
                yield return HandleChoices(line);
                continue;
            }

            bool nextPressed = false;

            ChoiceUI.Instance.OnNextPressed = () =>
            {
                StopAudioOnly();
                nextPressed = true;
            };

            ChoiceUI.Instance.Show(null);

            while (!nextPressed)
                yield return null;

            index = line.nextIndex >= 0 ? line.nextIndex : index + 1;
        }

        yield return Fade(0f);

        isPlaying = false;
    }

    // =========================
    // ITEM GATING
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

    bool CanPlay(VoiceLine line)
    {
        if (line.requiredItem == null)
            return true;

        if (!Inventory.Instance.HasItem(line.requiredItem))
            return false;

        if (line.consumeItem)
            Inventory.Instance.RemoveItem(line.requiredItem);

        return true;
    }

    // =========================
    // TYPE TEXT (FIXED PER-LETTER AUDIO)
    // =========================
    IEnumerator TypeText(VoiceLine line)
    {
        subtitleText.text = "";

        int soundCounter = 0;

        foreach (char c in line.subtitle)
        {
            subtitleText.text += c;

            // =========================
            // PER LETTER SFX (CONTROLLED)
            // =========================
            if (typingSFXSource != null)
            {
                soundCounter++;

                // every 2 letters = audible but still "per-letter feel"
                if (soundCounter % 2 == 0)
                {
                    typingSFXSource.Stop();
                    typingSFXSource.Play();
                }
            }

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
    // NEXT BUTTON
    // =========================
    public void OnNextLinePressed()
    {
        if (!isPlaying) return;

        StopAudioOnly();
    }

    void StopAudioOnly()
    {
        if (voiceSource != null && voiceSource.isPlaying)
            voiceSource.Stop();

        if (typingSFXSource != null && typingSFXSource.isPlaying)
            typingSFXSource.Stop();
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
            dialogueCanvasGroup.alpha =
                Mathf.Lerp(start, target, t / fadeDuration);

            yield return null;
        }

        dialogueCanvasGroup.alpha = target;
    }

    void HideUIInstant()
    {
        dialogueCanvasGroup.alpha = 0f;
    }

    // =========================
    // SETTINGS
    // =========================
    public void SetTextSpeed(float value)
    {
        textSpeed = Mathf.Max(0.1f, value);
    }
}