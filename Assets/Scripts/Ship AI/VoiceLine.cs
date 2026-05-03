using UnityEngine;

[System.Serializable]
public class VoiceLine
{
    [Header("Text")]
    public string speakerName = "SHIP AI";

    [TextArea(2, 5)]
    public string subtitle;

    [Header("Audio")]
    public AudioClip voiceClip;
    public AudioClip typingSFX;

    [Header("Typing")]
    public float baseTypeSpeed = 0.05f;   // default speed
    public float speedMultiplier = 1f;    // per-line modifier
    public bool overrideTyping = false;   // force manual speed
    public float overrideTypeSpeed = 0.05f;

    [Header("Timing")]
    public float fallbackDuration = 3f;

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