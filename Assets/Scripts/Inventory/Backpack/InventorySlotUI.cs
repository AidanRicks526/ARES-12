using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI")]
    public Image iconImage;
    public TextMeshProUGUI nameText;

    [Header("Data")]
    public InventorySlot slotData;

    private ItemInspectUI inspectUI;

    public void Setup(InventorySlot slot, ItemInspectUI inspect)
    {
        slotData = slot;
        inspectUI = inspect;

        if (slot.item != null)
        {
            iconImage.sprite = slot.item.icon;
            iconImage.enabled = true;

            nameText.text = slot.item.itemName;
        }
        else
        {
            iconImage.sprite = null;
            iconImage.enabled = false;

            nameText.text = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (slotData != null && slotData.item != null && inspectUI != null)
        {
            inspectUI.Open(slotData.item);
        }
    }
}