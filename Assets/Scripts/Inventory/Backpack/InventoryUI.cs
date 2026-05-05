using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject inventoryPanel;

    [Header("References")]
    public InventoryDisplay inventoryDisplay;



    private bool isOpen;

    void Start()
    {
        isOpen = false;
        inventoryPanel.SetActive(false);
    }

        void Update()
    {
        // Check for backpack press (true only on the frame the key was pressed)
        if (UserInput.WasBackpackPressed)
        {
            ToggleInventory();
        }
    }
    public void ToggleInventory()
    {
        isOpen = !isOpen;

        inventoryPanel.SetActive(isOpen);

        // Optional: pause game while open
        Time.timeScale = isOpen ? 0f : 1f;

        // Refresh when opening
        if (isOpen && inventoryDisplay != null)
        {
            inventoryDisplay.RefreshUI();
        }
    }
}