/*using UnityEngine;
using UnityEngine.InputSystem;

public class LabNotePickup : MonoBehaviour
{
    public LabNote noteData;          // Assign your LabNote asset here

    private bool _playerInRange;
    private Inventory _playerInventory; //Not used for pickup, but kept for consistency
    //private NoteDisplay noteDisplay;   // Reference to the UI display script

    /*private void Start()
    {
        // Find the NoteDisplay in the scene (persistent)
        noteDisplay = FindFirstObjectByType<NoteDisplay>();
        if (noteDisplay == null)
            Debug.LogError("NoteDisplay not found in scene!");
    }

    private void Update()
    {
        if (_playerInRange && UserInput.WasInteractPressed)
        {
            TryPickup();
            Destroy(gameObject); // Remove pickup from world
        }
    }

    private void TryPickup()
    {
        _playerInventory.AddItem(noteData);
        ShowNote();
        Destroy(gameObject); // Remove pickup from world
    }

    void ShowNote()
    {
        NoteDisplay.Instance.ShowNote(noteData);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
            _playerInventory = other.GetComponent<Inventory>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
            _playerInventory = null;
        }
    }
}*/