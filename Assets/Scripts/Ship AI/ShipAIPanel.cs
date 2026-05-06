using System.Collections.Generic;
using UnityEngine;

public class ShipAIPanel : MonoBehaviour
{
    [Header("Dialogue")]
    public VoiceLine[] lines;

    [Header("Reference")]
    public ShipAI shipAI;

    private bool playerInRange;

    void Awake()
    {
        if (shipAI == null)
            shipAI = FindFirstObjectByType<ShipAI>();
    }

    void Update()
    {
        if (!playerInRange || shipAI == null)
            return;

        if (UserInput.WasInteractPressed && !shipAI.IsPlaying)
        {
            TriggerDialogue();
        }
    }

    void TriggerDialogue()
    {
        if (lines == null || lines.Length == 0)
            return;

        shipAI.PlayDialogue(new List<VoiceLine>(lines));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}