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

    [Header("Timing")]
    public float fallbackDuration = 2f;
    public float typeSpeed = 0.03f;

    [Header("Inventory Requirement")]
    public ItemData requiredItem; // 🔥 direct reference to your item system

    [Header("Timer Requirement")]
    public float triggerBeforeTime = -1f;

    [Header("Branching")]
    public DialogueChoice[] choices;
}