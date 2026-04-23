using UnityEngine;

public class PuzzleInteract : MonoBehaviour
{
    public GameObject puzzleUI;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && UserInput.WasInteractPressed)
        {
            puzzleUI.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}