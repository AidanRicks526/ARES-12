using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI")]
    public Image iconImage;
    public TextMeshProUGUI quantityText;

    [Header("Data")]
    public InventorySlot slotData;

    public void Setup(InventorySlot slot)
    {
        slotData = slot;

        if (slot.item != null)
        {
            iconImage.sprite = slot.item.icon;
            iconImage.enabled = true;
        }
        else
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }

        quantityText.text = slot.quantity > 1 ? slot.quantity.ToString() : "";
    }

    // 🧠 CLICK HANDLER
    public void OnPointerClick(PointerEventData eventData)
    {
        if (slotData == null || slotData.item == null) return;

        ItemPopupUI.Instance.Show(slotData.item);
    }
}