using UnityEngine;
using UnityEngine.UI;

public class JigsawPuzzleManager : MonoBehaviour
{
    [Header("References")]
    public GameObject jigsawCanvas;   // The Canvas with background + close button
    public GameObject jigsawRoot;     // The GameObject holding JigsawBoard
    public Button closeButton;

    private bool isOpen = false;

    void Start()
    {
        closeButton.onClick.AddListener(ClosePuzzle);
        jigsawCanvas.SetActive(false);
        jigsawRoot.SetActive(false);
    }

    void Update()
    {
        // Temporary: press Space to open puzzle for testing
        if (Input.GetKeyDown(KeyCode.Space))
            OpenPuzzle();
    }

    public void OpenPuzzle()
    {
        isOpen = true;
        jigsawCanvas.SetActive(true);
        jigsawRoot.SetActive(true);
    }

    public void ClosePuzzle()
    {
        isOpen = false;
        jigsawCanvas.SetActive(false);
        jigsawRoot.SetActive(false);
    }
}