using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    public Inventory inventory;
    public ItemData testItem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            inventory.AddItem(testItem, 1);

            foreach (var slot in inventory.slots)
            {
                Debug.Log(slot.item.itemName + " x" + slot.quantity);
            }
        }
    }
}