using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class InventorySlot
{
    public ItemData item;
    public int quantity;
    public InventorySlot(ItemData item, int qty) { this.item = item; this.quantity = qty; }
}