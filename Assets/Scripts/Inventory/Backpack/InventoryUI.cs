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

    // =========================
    // BUTTON CALL (UI ONLY)
    // =========================
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