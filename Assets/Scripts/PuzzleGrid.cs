using UnityEngine;

public class PuzzleGrid : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform gridParent;

    public GameObject puzzlePanel;

    private Tile[,] tiles = new Tile[3, 3];

    void OnEnable()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                GameObject obj = Instantiate(tilePrefab, gridParent);
                Tile tile = obj.GetComponent<Tile>();

                tile.Init(this, x, y);

                tiles[x, y] = tile;
            }
        }

        RandomizeGrid();
    }

    void RandomizeGrid()
    {
        for (int i = 0; i < 10; i++)
        {
            int x = Random.Range(0, 3);
            int y = Random.Range(0, 3);

            FlipTiles(x, y);
        }
    }

    public void OnTileClicked(int x, int y)
    {
        FlipTiles(x, y);

        CheckWin();
    }

    void FlipTiles(int x, int y)
    {
        ToggleTile(x, y);
        ToggleTile(x + 1, y);
        ToggleTile(x - 1, y);
        ToggleTile(x, y + 1);
        ToggleTile(x, y - 1);
    }

    void ToggleTile(int x, int y)
    {
        if (x < 0 || x >= 3 || y < 0 || y >= 3)
            return;

        tiles[x, y].Toggle();
    }

    void CheckWin()
    {
        bool allOn = true;

        foreach (var tile in tiles)
        {
            if (!tile.isOn)
            {
                allOn = false;
                break;
            }
        }

        if (allOn)
        {
            Debug.Log("Puzzle Solved!");

            ClosePuzzle();
        }
    }
    void ClosePuzzle()
    {
        puzzlePanel.SetActive(false);
    }
}