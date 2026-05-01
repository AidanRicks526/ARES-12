using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class JigsawPuzzleManager : MonoBehaviour
{
    [Header("References")]
    public GameObject jigsawCanvas;   // The Canvas with background + close button
    public GameObject jigsawRoot;     // The GameObject holding JigsawBoard
    public Button closeButton;
    public float fadeDuration = 1.5f; // e.g. 1.5 seconds 

    public GameObject lightsUI;       // Drag the Lights_UI GameObject here in the Inspector

    private bool isOpen = false;
    public CanvasGroup textOverlayGroup;
    private JigsawInteraction[] allTiles;
    private Light2D light2D;

    // Camera state we restore when the puzzle closes
    private SimpleCameraFollow cameraFollow;
    private Vector3 cameraSavedPosition;
    private bool cameraFollowWasEnabled;


    void Start()
    {
        closeButton.onClick.AddListener(ClosePuzzle);
        jigsawCanvas.SetActive(false);
        jigsawRoot.SetActive(false);

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            light2D = player.GetComponentInChildren<Light2D>(true);
        if (light2D == null)
            Debug.LogWarning("JigsawPuzzleManager could not find a Light2D under the Player.", this);
    }

    void Update()
    {
        // Temporary: press Space to open puzzle for testing
        if (Input.GetKeyDown(KeyCode.Space))
            OpenPuzzle();

        // DEBUG: Press P to instantly complete the puzzle
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
        }
        #endif

    }

    public void OpenPuzzle()
    {
        if (isOpen) return;// Guard against double opening
        isOpen = true;
        LockerInteract.IsUIOpenGlobal = true;   // freeze player movement

        // Snap the camera to (0, 0) so the world-space board renders centered.
        // Disable the follow script so it doesn't drag the camera back to the player.
        if (Camera.main != null)
        {
            cameraFollow = Camera.main.GetComponent<SimpleCameraFollow>();
            cameraSavedPosition = Camera.main.transform.position;
            if (cameraFollow != null)
            {
                cameraFollowWasEnabled = cameraFollow.enabled;
                cameraFollow.enabled = false;
            }
            // Keep the camera's existing z (typically -10 for 2D)
            Camera.main.transform.position = new Vector3(0f, 0f, cameraSavedPosition.z);
        }

        jigsawCanvas.SetActive(true);
        jigsawRoot.SetActive(true);
        if (light2D != null) light2D.enabled = false;
        if (lightsUI != null) lightsUI.SetActive(false);

        // Grab tiles after OnEnable generates them
        allTiles = jigsawRoot.GetComponentsInChildren<JigsawInteraction>();

        // Match the overlay canvas to the board's world size
        /*JigsawBoard board = jigsawRoot.GetComponentInChildren<JigsawBoard>();
        if (board != null && textOverlayGroup != null)
        {
            RectTransform rt = textOverlayGroup.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.sizeDelta = new Vector2(board.boardWorldWidth, board.boardWorldHeight);
                rt.position = board.boardCenter;
            }
        }*/
    }

    private void OnDisable()
    {
        if (isOpen) LockerInteract.IsUIOpenGlobal = false;
    }

    public void CheckCompletion()
    {
        foreach (var tile in allTiles)
        {
            if (!tile.IsSnapped) return; // Exit if any piece isn't snapped
        }
        StartCoroutine(RevealText()); // All pieces snapped — reveal text
    }

    public void ClosePuzzle()
    {
        isOpen = false;
        LockerInteract.IsUIOpenGlobal = false;  // unfreeze player movement
        jigsawCanvas.SetActive(false);
        jigsawRoot.SetActive(false);
        if (light2D != null) light2D.enabled = true;
        if (lightsUI != null) lightsUI.SetActive(true);

        // Restore the camera to where it was before the puzzle opened.
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
            textOverlayGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
        textOverlayGroup.alpha = 1f; // Guarantee full opacity
    }
}