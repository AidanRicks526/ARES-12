using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [Tooltip("Drag the Image object meant for the Item Icon here")]
    public Image iconImage;

    [Tooltip("Drag the TextMeshPro object for the Quantity here")]
    public TextMeshProUGUI quantityText;
}