using UnityEngine;
using TMPro;

public class LockerInteract : MonoBehaviour
{
    public static bool IsUIOpenGlobal { get; private set; }

    [Header("Settings")]
    public string correctPassword = "A123";

    [Header("References")]
    public string playerTag = "Player";

    [Header("Save ID (runtime only)")]
    public string lockerID = "Locker_01";

    private Transform player;

    [Header("UI")]
    public GameObject uiPanel;
    public TMP_InputField inputField;
    public TextMeshProUGUI feedbackText;

    [Header("Reward")]
    public GameObject itemToSpawn;
    public Transform spawnPoint;

    private bool isPlayerTouching;
    private bool isUnlocked;
    private bool isUIOpen;

    void Start()
    {
        // reset UI state
        IsUIOpenGlobal = false;
        isUIOpen = false;

        if (uiPanel != null)
            uiPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // LOAD runtime save (NOT persistent)
        isUnlocked = Locker_Runtime_Save.IsUnlocked(lockerID);

        if (isUnlocked)
        {
            Destroy(gameObject);
            return;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
            player = playerObj.transform;

        if (itemToSpawn != null)
            itemToSpawn.SetActive(false);
    }

    void Update()
    {
        if (player == null || isUnlocked) return;

        if (isPlayerTouching && !isUIOpen && Input.GetKeyDown(KeyCode.E))
        {
            OpenUI();
        }
    }

    void OpenUI()
    {
        isUIOpen = true;
        IsUIOpenGlobal = true;

        if (uiPanel != null)
            uiPanel.SetActive(true);

        if (inputField != null)
            inputField.text = "";

        if (feedbackText != null)
            feedbackText.text = "";

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SubmitPassword()
    {
        if (isUnlocked) return;

        string entered = inputField.text;

        if (entered == correctPassword)
        {
            feedbackText.text = "ACCESS GRANTED";
            UnlockLocker();
        }
        else
        {
            feedbackText.text = "ERROR";
            inputField.text = "";
        }
    }

    void UnlockLocker()
    {
        if (isUnlocked) return;

        isUnlocked = true;

        // SAVE ONLY FOR THIS SESSION
        Locker_Runtime_Save.Unlock(lockerID);

        CloseUI();

        if (itemToSpawn != null)
        {
            if (spawnPoint != null)
                itemToSpawn.transform.position = spawnPoint.position;

            itemToSpawn.SetActive(true);
        }

        Destroy(gameObject);
    }

    public void CloseUI()
    {
        isUIOpen = false;
        IsUIOpenGlobal = false;

        if (uiPanel != null)
            uiPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
            isPlayerTouching = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerTouching = false;

            if (isUIOpen)
                CloseUI();
        }
    }
}