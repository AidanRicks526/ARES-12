using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public int maxSlots = 20;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public Action OnInventoryChanged;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // optional, if you want it to persist
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public bool AddItem(ItemData item, int amount = 1)
    {
        if (item.stackable)
        {
            foreach (var slot in slots)
            {
                if (slot.item == item && slot.quantity < item.maxStack)
                {
                    int toAdd = Mathf.Min(item.maxStack - slot.quantity, amount);
                    slot.quantity += toAdd;
                    amount -= toAdd;
                    if (amount <= 0) break;
                }
            }
        }

        while (amount > 0 && slots.Count < maxSlots)
        {
            int toAdd = item.stackable ? Mathf.Min(item.maxStack, amount) : 1;
            slots.Add(new InventorySlot(item, toAdd));
            amount -= toAdd;
        }

        OnInventoryChanged?.Invoke();
        return amount <= 0;
    }

    public void RemoveItem(ItemData item, int amount = 1)
    {
        for (int i = slots.Count - 1; i >= 0; i--)
        {
            if (slots[i].item == item)
            {
                slots[i].quantity -= amount;
                if (slots[i].quantity <= 0) slots.RemoveAt(i);
                break;
            }
        }
        OnInventoryChanged?.Invoke();
    }

    public bool HasItem(ItemData item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == item && slot.quantity > 0)
                return true;
        }
        return false;
    }

    // Overload version (keeps old system working safely)
    public bool HasItem(ItemData item, int amount)
    {
        int total = 0;

        foreach (InventorySlot slot in slots)
        {
            if (slot.item == item)
                total += slot.quantity;
        }

        return total >= amount;
    }
}