using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPopupUI : MonoBehaviour
{
    public static ItemPopupUI Instance;

    public GameObject panel;
    public Image icon;
    public TextMeshProUGUI nameText;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(ItemData item)
    {
        if (item == null) return;

        panel.SetActive(true);
        icon.sprite = item.icon;
        nameText.text = item.itemName;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}