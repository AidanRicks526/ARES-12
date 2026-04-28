using UnityEngine;
using System.Collections; 

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;
    public int amount = 1;
    public float pickupRange = 2.5f;

    [Header("Puzzle Settings")]
    //public GameObject puzzleUI; // Drag Puzzle Panel here
    public JigsawPuzzleManager puzzleManager; // drag JigsawPuzzleManager here
    public float delayBeforePuzzle = 2.0f;

    [Header("Note Settings")]
    public LabNote noteData;

    private bool _isBeingPickedUp = false;

    private void Update()
    {
        if (_isBeingPickedUp) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        if (Vector3.Distance(transform.position, player.transform.position) <= pickupRange)
        {
            if (UserInput.WasInteractPressed)
            {
                StartCoroutine(PickupRoutine(player.GetComponent<Inventory>()));
        }
    }
    }

    private IEnumerator PickupRoutine(Inventory inv)
    {
        if (inv == null) yield break;

        _isBeingPickedUp = true;

        // 1. Add to inventory
        if (itemData != null)
            inv.AddItem(itemData, amount);

        // 2.Show note if this is a note pickup
        if (noteData != null)
        {
            inv.AddItem(noteData);
            NoteDisplay.Instance.ShowNote(noteData);
        }

        // 2. Play Animation 
        //Animator anim = GetComponent<Animator>();
        //if (anim != null) anim.SetTrigger("Collect");

        // 3. Wait for the animation
        yield return new WaitForSeconds(delayBeforePuzzle);

        // 4. Hide the shard
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // 5. Pop up the puzzle
        if (puzzleManager != null)
        {
            puzzleManager.OpenPuzzle();
        }

        // 5. Pop up the puzzle
        /*if (puzzleUI != null)
        {
            puzzleUI.SetActive(true);
        }*/


        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.CompareTag("Player"))
        //    {
        //        _playerInRange = true;
        //        _playerInventory = other.GetComponent<Inventory>();
        //    }
        //}
        
        // 6. destroy this object
        Destroy(gameObject, 0.1f);
    }
}
