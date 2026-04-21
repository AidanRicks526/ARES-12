using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel; // UI panel root
    //public Transform slotParent; //Parent object where slots are instantiate -sg
    //public GameObject slotPrefab; //A UI prefab representing an inventory slot -sg
    private bool isOpen = false;

    void Start()
    {
        inventoryPanel.SetActive(false);
        //RefreshUI(); //Call this after inventory changes -sg
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);
        Time.timeScale = isOpen ? 0f : 1f; // Optional: lock/unlock player input

        //if (isOpen) //sg
            //RefreshUI(); //sg
    }

    /*public void RefreshUI()
    {
        //-sg
        if (slotParent == null)
        {
            Debug.LogError("slotParent is not assigned in InventoryUI!");
            return;
        }
        //Clear old slots -sg
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        Inventory inv = FindFirstObjectByType<Inventory>(); //Or reference via singleton -sg

        if (inv == null) return;

        foreach (var slot in inv.slots)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotParent); //Assume your slot has an Image for the icon and text for quantity -sg

            slotObj.GetComponentInChildren<Image>().sprite = slot.item.icon;
            slotObj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = slot.quantity.ToString();

            //Add a button or click handler - sg
            Button btn = slotObj.GetComponent<Button>();
            btn.onClick.AddListener(() => OnItemClicked(slot.item)); //For right-click you can use a custom script or IPointerClickHandler -sg
        }
    }

    private void OnItemClicked(ItemData item)
    {
        if (item is LabNote labNote)
        {
            // Open the note reading UI -sg
            FindFirstObjectByType<NoteDisplay>().ShowNote(labNote);
        }
        else
        {
            Debug.Log($"Used {item.itemName} (no special action)");
        }
    }*/
}