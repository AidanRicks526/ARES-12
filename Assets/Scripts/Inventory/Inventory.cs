using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private ItemDatabase itemDatabase; // Drag .asset ScriptableObject here in Inspector

    public int maxSlots = 20;
    public List<InventorySlot> slots = new List<InventorySlot>();

    public bool AddItemById(string itemId, int amount = 1)
    {
        if (itemDatabase == null)
        {
            Debug.LogError("ItemDatabase not assigned to Inventory!");
            return false;
        }

        ItemData item = itemDatabase.GetById(itemId);
        if (item == null)
        {
            Debug.LogWarning($"Item with ID '{itemId}' not found in database.");
            return false;
        }

        return AddItem(item, amount);
    }

    public bool AddItem(ItemData item, int amount = 1)
    {
        // Try stacking first
        if (item.stackable)
        {
            foreach (var slot in slots)
            {
                if (slot.item == item && slot.quantity < item.maxStack)
                {
                    int spaceLeft = item.maxStack - slot.quantity;
                    int toAdd = Mathf.Min(spaceLeft, amount);

                    slot.quantity += toAdd;
                    amount -= toAdd;

                    if (amount <= 0)
                        return true;
                }
            }
        }

        // Add new slot(s)
        while (amount > 0)
        {
            if (slots.Count >= maxSlots)
                return false;

            int toAdd = item.stackable ? Mathf.Min(item.maxStack, amount) : 1;

            slots.Add(new InventorySlot(item, toAdd));
            amount -= toAdd;
        }

        return true;
    }

    public void RemoveItem(ItemData item, int amount = 1)
    {
        for (int i = slots.Count - 1; i >= 0; i--)
        {
            if (slots[i].item == item)
            {
                slots[i].quantity -= amount;

                if (slots[i].quantity <= 0)
                {
                    amount = -slots[i].quantity;
                    slots.RemoveAt(i);
                }
                else
                {
                    return;
                }
            }
        }
    }
}