using UnityEngine;
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

    void OnEnable()
    {
        RefreshUI();
    }

    void Start()
    {
        inventory.OnInventoryChanged += RefreshUI;
        RefreshUI();
    }

    public void RefreshUI()
    {
        // Clear old UI
        foreach (var obj in spawnedSlots)
            Destroy(obj);

        spawnedSlots.Clear();

        // Rebuild UI
        foreach (var slot in inventory.slots)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotParent);
            spawnedSlots.Add(newSlot);

            InventorySlotUI slotUI = newSlot.GetComponent<InventorySlotUI>();

            if (slotUI != null)
            {
                slotUI.Setup(slot);
            }
        }
    }
}