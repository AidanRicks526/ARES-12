using System.Collections.Generic;
using UnityEngine;

public class ShipAIPanel : MonoBehaviour
{
    public VoiceLine[] lines;

    private bool playerInRange;
    private ShipAI shipAI;

    void Start()
    {
        shipAI = FindObjectOfType<ShipAI>();
    }

    void Update()
    {
        if (playerInRange && UserInput.WasInteractPressed && !shipAI.IsPlaying)
        {
            Trigger();
        }
    }

    void Trigger()
    {
        List<VoiceLine> valid = new List<VoiceLine>();

        foreach (var line in lines)
        {
            valid.Add(line);
        }

        shipAI.PlayDialogue(valid);
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