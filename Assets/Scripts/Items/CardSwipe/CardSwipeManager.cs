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
            DontDestroyOnLoad(gameObject);   // <-- ADD THIS LINE
        }
      
        else Destroy(gameObject);
    }

    void Start()
    {
        swipePanel.SetActive(false);
    }

    /// <summary>
    /// Opens the swipe panel for the given door.
    /// Checks the inventory for the required badge and sets up the UI accordingly.
    /// </summary>
    public void ShowSwipePanel(DoorTriggerInteraction door, ItemData requiredBadge)
    {
        currentDoor = door;
        currentRequiredBadge = requiredBadge;
        swipePanel.SetActive(true);

        if (Inventory.Instance != null && Inventory.Instance.HasItem(requiredBadge))
        {
            // Player has the badge → show the draggable card
            badgeCardImage.gameObject.SetActive(true);
            messageText.gameObject.SetActive(false);
            ResetCardPosition();
        }
        else
        {
            // No badge → show warning and auto‑close after a delay
            badgeCardImage.gameObject.SetActive(false);
            messageText.gameObject.SetActive(true);
            messageText.text = "You need an ID Badge";
            Invoke(nameof(HidePanel), 2f);
        }
    }

    void ResetCardPosition()
    {
        badgeCardRect.anchoredPosition = new Vector2(-300f, 0f);
    }

    /// <summary>
    /// Called by DraggableCard when the swipe is successful.
    /// Unlocks the current door and hides the panel.
    /// </summary>
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