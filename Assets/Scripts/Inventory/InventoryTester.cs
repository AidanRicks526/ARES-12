using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    public Inventory inventory;
    public ItemData testItem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventory.AddItem(testItem, 1);
            Debug.Log("Item added");
        }
    }
}