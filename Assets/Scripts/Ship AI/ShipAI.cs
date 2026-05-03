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

    public void SetTextSpeed(float value)
    {
        textSpeed = Mathf.Max(0.1f, value);
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

        while (index >= 0 && index < currentDialogue.Count)
        {
            VoiceLine line = currentDialogue[index];

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
                index = line.choices[index].nextIndex;
                continue;
            }

            // =========================
            // NEXT BUTTON FLOW (FIXED)
            // =========================
            bool nextPressed = false;

            ChoiceUI.Instance.OnNextPressed = () =>
            {
                if (voiceSource != null && voiceSource.isPlaying)
                    voiceSource.Stop();

                nextPressed = true;
            };

            ChoiceUI.Instance.Show(null);

            // Wait until voice finishes naturally
            while (voiceSource != null && voiceSource.isPlaying)
                yield return null;

            // NOW wait for player input (NO auto-skip possible)
            yield return new WaitUntil(() => nextPressed);

            index = line.nextIndex >= 0 ? line.nextIndex : index + 1;
        }

        yield return Fade(0f);
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

            yield return new WaitForSeconds(0.05f / textSpeed);
        }
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

    public void OnNextLinePressed()
    {
        if (!isPlaying) return;

        StopAllCoroutines();
        isPlaying = false;
        dialogueCanvasGroup.alpha = 0f;
    }
}