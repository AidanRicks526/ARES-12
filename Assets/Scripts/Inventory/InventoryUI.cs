using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel; // UI panel root
    private bool isOpen = false;

    void Start()
    {
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        // Optional: lock/unlock player input
        Time.timeScale = isOpen ? 0f : 1f;
    }
}