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
    public AudioClip typingSFX; // optional beep sound

    [Header("Timing")]
    public float fallbackDuration = 2f;
    public float typeSpeed = 0.03f;
}