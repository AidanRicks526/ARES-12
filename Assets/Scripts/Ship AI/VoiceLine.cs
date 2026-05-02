using UnityEngine;

[System.Serializable]
public class VoiceLine
{
    [Header("Identity")]
    public string speakerName = "SHIP AI";

    [TextArea(2, 5)]
    public string subtitle;

    [Header("Audio")]
    public AudioClip voiceClip;
    public AudioClip typingSFX;

    [Header("Timing")]
    public float fallbackDuration = 2f;
    public float typeSpeed = 0.03f;

    [Header("Progression")]
    public int nextIndex = -1;

    [Header("Restrictions")]
    public ItemData requiredItem;
    public bool consumeItem = false;

    [Header("Timer Gate")]
    public bool useTimerGate = false;
    public float triggerBeforeTime = 0f;

    [Header("Branching")]
    public DialogueChoice[] choices;
}