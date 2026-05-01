using UnityEngine;
using UnityEngine.InputSystem;

public class NewItem_PickUp : MonoBehaviour
{
    [Header("Item")]
    public ItemData itemData;

    [Header("Equip Settings")]
    public bool autoEquip = false;

    private bool _playerInRange;
    private Inventory _playerInventory;

    private void Start()
    {
        /*if (itemData == null) return;
        if (GameManager.Instance != null && GameManager.Instance.IsCollected(itemData))
            gameObject.SetActive(false);*/
    }

    private void Update()
    {
        if (_playerInRange && UserInput.WasInteractPressed)
        {
            TryPickup();
        }
    }

    private void TryPickup()
    {
        if (_playerInventory == null || itemData == null)
            return;

        bool added = _playerInventory.AddItem(itemData);

        if (!added)
        {
            Debug.Log("Inventory full!");
            return;
        }

        if (autoEquip)
        {
            EquipItem();
        }

        /*if (GameManager.Instance != null) // SG code
            GameManager.Instance.RegisterCollected(itemData);*/

        Destroy(gameObject);
    }

    void EquipItem()
    {
        Debug.Log($"Equipped: {itemData.itemName}");

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
}