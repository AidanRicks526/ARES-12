using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class JigsawPuzzleManager : MonoBehaviour
{
    [Header("References")]
    public GameObject jigsawCanvas;
    public GameObject jigsawRoot;
    public Button closeButton;
    public float fadeDuration = 1.5f;

    public GameObject lightsUI;

    private bool isOpen = false;
    public CanvasGroup textOverlayGroup;
    private JigsawInteraction[] allTiles;
    private Light2D light2D;

    private SimpleCameraFollow cameraFollow;
    private Vector3 cameraSavedPosition;
    private bool cameraFollowWasEnabled;

    [Header("Panel Settings")]
    public Image puzzlePanel;
    [Range(0f, 1f)]
    public float startingAlpha = 0.5f; // Set this to 0.5 in Inspector for 50% transparency


    void Start()
    {
        closeButton.onClick.AddListener(ClosePuzzle);
        jigsawCanvas.SetActive(false);
        jigsawRoot.SetActive(false);

        // Initialize at your preferred transparency
        SetPanelAlpha(startingAlpha);

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            light2D = player.GetComponentInChildren<Light2D>(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            OpenPuzzle();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P) && isOpen)
        {
            foreach (var tile in allTiles)
            {
                if (!tile.IsSnapped)
                {
                    tile.transform.position = tile.CorrectPosition;
                    tile.LockPiece();
                }
            }
            CheckCompletion();
        }
#endif
    }

    public void OpenPuzzle()
    {
        if (isOpen) return;
        isOpen = true;
        LockerInteract.IsUIOpenGlobal = true;

        if (Camera.main != null)
        {
            cameraFollow = Camera.main.GetComponent<SimpleCameraFollow>();
            cameraSavedPosition = Camera.main.transform.position;
            if (cameraFollow != null)
            {
                cameraFollowWasEnabled = cameraFollow.enabled;
                cameraFollow.enabled = false;
            }
            Camera.main.transform.position = new Vector3(0f, 0f, cameraSavedPosition.z);
        }

        jigsawCanvas.SetActive(true);
        jigsawRoot.SetActive(true);

        // Force the panel to start at 50% transparency (0.5f alpha)
        if (puzzlePanel != null)
        {
            puzzlePanel.gameObject.SetActive(true);
            SetPanelAlpha(startingAlpha);
        }

        if (light2D != null) light2D.enabled = false;
        if (lightsUI != null) lightsUI.SetActive(false);

        allTiles = jigsawRoot.GetComponentsInChildren<JigsawInteraction>();
    }

    private void SetPanelAlpha(float alphaValue)
    {
        if (puzzlePanel != null)
        {
            Color tempColor = puzzlePanel.color;
            tempColor.a = alphaValue;
            puzzlePanel.color = tempColor;
        }
    }

    public void CheckCompletion()
    {
        foreach (var tile in allTiles)
        {
            if (!tile.IsSnapped) return;
        }
        StartCoroutine(RevealText());
    }

    public void ClosePuzzle()
    {
        isOpen = false;
        LockerInteract.IsUIOpenGlobal = false;
        jigsawCanvas.SetActive(false);
        jigsawRoot.SetActive(false);
        if (light2D != null) light2D.enabled = true;
        if (lightsUI != null) lightsUI.SetActive(true);

        if (Camera.main != null)
        {
            Camera.main.transform.position = cameraSavedPosition;
            if (cameraFollow != null) cameraFollow.enabled = cameraFollowWasEnabled;
        }
    }

    IEnumerator RevealText()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;

            if (textOverlayGroup != null)
                textOverlayGroup.alpha = Mathf.Lerp(0f, 1f, t);

            // CHANGED: Lerp from startingAlpha (0.5) to 0 (Fully Transparent)
            if (puzzlePanel != null)
            {
                SetPanelAlpha(Mathf.Lerp(startingAlpha, 0f, t));
            }

            yield return null;
        }

        if (textOverlayGroup != null) textOverlayGroup.alpha = 1f;

        if (puzzlePanel != null)
        {
            SetPanelAlpha(0f);
            puzzlePanel.gameObject.SetActive(false);
        }
    }
}