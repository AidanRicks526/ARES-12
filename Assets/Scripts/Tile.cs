using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;

    public bool isOn = false;

    private PuzzleGrid grid;
    private Image image;

    [Header("Tile Sprites")]
    public Sprite onSprite;
    public Sprite offSprite;

    public void Init(PuzzleGrid gridRef, int xPos, int yPos)
    {
        grid = gridRef;
        x = xPos;
        y = yPos;

        image = GetComponent<Image>();

        GetComponent<Button>().onClick.AddListener(OnClick);

        UpdateVisual();
    }

    void OnClick()
    {
        grid.OnTileClicked(x, y);
    }

    public void Toggle()
    {
        isOn = !isOn;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (image == null) return;

        image.sprite = isOn ? onSprite : offSprite;
    }
}