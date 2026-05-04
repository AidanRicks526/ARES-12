using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemData item;
    public int quantity;

    public InventorySlot(ItemData item, int qty)
    {
        this.item = item;
        this.quantity = qty;
    }
}