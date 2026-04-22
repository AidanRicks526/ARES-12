using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryDisplay : MonoBehaviour
{
    public Inventory inventory;
    public Transform slotParent;
    public GameObject slotPrefab;

    private List<GameObject> spawnedSlots = new List<GameObject>();

    void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<Inventory>();
    }
    void Start()
    {
        // subscribe to inventory changes
        inventory.OnInventoryChanged += RefreshUI;
        Debug.Log("UI Refresh running. Inventory slots: " + inventory.slots.Count);
        RefreshUI(); // initial draw
    }

    public void RefreshUI()
    {
        Debug.Log("Refreshing UI. Slots: " + inventory.slots.Count);

        // Clear old UI
        foreach (var obj in spawnedSlots)
        {
            Destroy(obj);
        }
        spawnedSlots.Clear();

        // Rebuild UI
        foreach (var slot in inventory.slots)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotParent);
            spawnedSlots.Add(newSlot);

            // Try to find components safely
            Image icon = newSlot.GetComponentInChildren<Image>();
            // This looks for ANY image in the prefab, which is safer than searching by name "Icon"

            if (icon != null) icon.sprite = slot.item.icon;

            // Use TMPro if you are using the modern Unity Text
            var qty = newSlot.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (qty != null) qty.text = slot.quantity.ToString();
        }
    }
}