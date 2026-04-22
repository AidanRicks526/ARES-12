using UnityEngine;

public class ShipAIPanel : MonoBehaviour
{
    public VoiceLine[] lines;

    private bool playerInRange = false;
    private ShipAI shipAI;

    void Start()
    {
        shipAI = FindObjectOfType<ShipAI>();
    }

    void Update()
    {
        if (playerInRange && UserInput.WasInteractPressed && !shipAI.IsPlaying)
        {
            TriggerDialogue();
        }
    }

    void TriggerDialogue()
    {
        foreach (var line in lines)
        {
            shipAI.PlayVoiceLine(line);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}