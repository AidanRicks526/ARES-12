using UnityEngine;
using UnityEngine.UI;

public class ItemInspectUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;
    public Image inspectImage;

    private ItemData currentItem;

    void Start()
    {
        panel.SetActive(false);
    }

    public void Open(ItemData item)
    {
        currentItem = item;

        if (currentItem != null && currentItem.inspectImage != null)
        {
            inspectImage.sprite = currentItem.inspectImage;
        }

        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Close()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}