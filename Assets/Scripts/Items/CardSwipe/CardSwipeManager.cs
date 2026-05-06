using UnityEngine;
using UnityEngine.UI;

public class CardSwipeManager : MonoBehaviour
{
    public static CardSwipeManager Instance;

    [Header("UI References")]
    public GameObject swipePanel;
    public RectTransform badgeCardRect;
    public RectTransform sensorRect;
    public Text messageText;
    public Image badgeCardImage;

    private DoorTriggerInteraction currentDoor;
    private ItemData currentRequiredBadge;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        swipePanel.SetActive(false);
    }

    public void ShowSwipePanel(DoorTriggerInteraction door, ItemData requiredBadge)
    {
        currentDoor = door;
        currentRequiredBadge = requiredBadge;

        if (Inventory.Instance != null && Inventory.Instance.HasItem(requiredBadge))
        {
            // Player HAS badge → show swipe UI
            swipePanel.SetActive(true);
            badgeCardImage.gameObject.SetActive(true);
            messageText.gameObject.SetActive(false);
            ResetCardPosition();
        }
        else
        {
            // ❌ Player DOES NOT have badge → show popup instead
            DoorLockedUI.Instance?.Show("Door Locked");
            HidePanel();
        }
    }

    void ResetCardPosition()
    {
        badgeCardRect.anchoredPosition = new Vector2(-300f, 0f);
    }

    public void OnSwipeSuccess()
    {
        Debug.Log("Swipe success!");

        if (currentDoor != null)
            currentDoor.UnlockDoor();

        HidePanel();
    }

    public void HidePanel()
    {
        swipePanel.SetActive(false);
    }
}