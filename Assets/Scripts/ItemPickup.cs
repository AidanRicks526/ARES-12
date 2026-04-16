using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;
    public int amount = 1;

    private bool _playerInRange;
    private Inventory _playerInventory;

    private void Update()
    {
        if (_playerInRange && UserInput.WasInteractPressed)
        {
            TryPickup();
        }
    }

    private void TryPickup()
    {
        if (_playerInventory == null) return;

        bool added = _playerInventory.AddItem(itemData, amount);

        if (added)
        {
            Destroy(gameObject); // remove item from world
        }
        else
        {
            Debug.Log("Inventory Full");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
            _playerInventory = other.GetComponent<Inventory>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
            _playerInventory = null;
        }
    }
}
