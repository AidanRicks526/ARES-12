using UnityEngine;
using System.Collections.Generic;

public class InventoryDisplay : MonoBehaviour
{
    public Inventory inventory;
    public Transform slotParent;
    public GameObject slotPrefab;

    [Header("References")]
    public ItemInspectUI inspectUI;

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
        foreach (var obj in spawnedSlots)
            Destroy(obj);

        spawnedSlots.Clear();

        foreach (var slot in inventory.slots)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotParent);
            spawnedSlots.Add(newSlot);

            InventorySlotUI slotUI = newSlot.GetComponent<InventorySlotUI>();

            if (slotUI != null)
            {
                slotUI.Setup(slot, inspectUI);
            }
        }
    }
}