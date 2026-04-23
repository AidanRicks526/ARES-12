using UnityEngine;
using TMPro;

public class LockerInteract : MonoBehaviour
{
    public static bool IsUIOpenGlobal { get; private set; }

    [Header("Settings")]
    public string correctPassword = "A123";

    [Header("References")]
    public string playerTag = "Player";

    private Transform player;

    public GameObject uiPanel;
    public TMP_InputField inputField;
    public TextMeshProUGUI feedbackText;

    public GameObject itemToSpawn;
    public Transform spawnPoint;

    private bool isPlayerTouching = false;
    private bool isUnlocked = false;
    private bool isUIOpen = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
            player = playerObj.transform;

        uiPanel.SetActive(false);
        itemToSpawn.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        if (isPlayerTouching && !isUIOpen && Input.GetKeyDown(KeyCode.E))
        {
            OpenUI();
        }
    }

    void OpenUI()
    {
        isUIOpen = true;
        IsUIOpenGlobal = true;

        uiPanel.SetActive(true);
        inputField.text = "";
        feedbackText.text = "";

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SubmitPassword()
    {
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

        if (itemToSpawn != null)
        {
            if (spawnPoint != null)
                itemToSpawn.transform.position = spawnPoint.position;

            itemToSpawn.SetActive(true);
        }
    }

    public void CloseUI()
    {
        isUIOpen = false;
        IsUIOpenGlobal = false;

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