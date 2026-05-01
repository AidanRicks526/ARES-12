using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;

    [Tooltip("Index of next VoiceLine in sequence")]
    public int nextIndex = -1;
}