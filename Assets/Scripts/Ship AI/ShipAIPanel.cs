using System.Collections.Generic;
using UnityEngine;

public class ShipAIPanel : MonoBehaviour
{
    [Header("Dialogue Lines")]
    public VoiceLine[] lines;

    [Header("Reference")]
    public ShipAI shipAI;

    private bool playerInRange;

    void Awake()
    {
        // fallback safety (but assign manually preferred)
        if (shipAI == null)
            shipAI = FindFirstObjectByType<ShipAI>();
    }

    void Update()
    {
        if (!playerInRange) return;
        if (shipAI == null) return;

        if (UserInput.WasInteractPressed && !shipAI.IsPlaying)
        {
            TriggerDialogue();
        }
    }

    void TriggerDialogue()
    {
        List<VoiceLine> sequence = new List<VoiceLine>(lines);

        shipAI.PlayDialogue(sequence);
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