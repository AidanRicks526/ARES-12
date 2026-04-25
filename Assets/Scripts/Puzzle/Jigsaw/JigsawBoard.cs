using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class JigsawBoard : MonoBehaviour
{
    [Header("Puzzle Configuration")]
    public GameObject tilePrefab; // Drag JigsawTile prefab here
    public Texture2D puzzleImage; // The full puzzle texture
    public int totalPieces = 12;  // Number of pieces
    public bool autoCalculate = true; // Manual override
    public int manualColumns = 4, manualRows = 4;// Manual override
    public bool maintainAspectRatio = true; // keep pieces square-ish

    private int columns, rows;
    //private int[,] hEdges; // Horizontal seams: [x, y] connects tile(x, y) bottom to tile(x, y+1) top
    //private int[,] vEdges; // Vertical seams:   [x, y] connects tile(x, y) right to tile(x+1, y) left
    private List<float>[,] hEdgeOffsets; // [columns, rows - 1]
    private List<float>[,] vEdgeOffsets; // [columns - 1, rows]

    [Header("Sizes")]
    public float tileWidth = 1.0f;       // World‑space width of each piece
    public float tileHeight = 1.0f;      // World‑space height of each piece
    public int curveResolution = 8;   // Must match the tile's curveResolution

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (autoCalculate)
            CalculateGrid();
        else
        {
            columns = manualColumns;
            rows = manualRows;
        }
        AutoSizeToScreen();
        GenerateEdgeTypes();
        GenerateBoard();
    }

    void CalculateGrid()
    {
        // Image dimensions (so we respect aspect ratio)
        float imgWidth = puzzleImage.width;
        float imgHeight = puzzleImage.height;
        float aspect = imgWidth / imgHeight;

        // Start with a guess based on total pieces
        int targetColumns = Mathf.RoundToInt(Mathf.Sqrt(totalPieces * aspect));
        int targetRows = Mathf.CeilToInt((float)totalPieces / targetColumns);

        // Make sure it don't exceed the total piece count (or allow slightly more)
        // Can adjust the last row later, but for now clamp to total pieces
        while (targetColumns * targetRows > totalPieces)
            targetRows--;

        // Ensure there is at least one column and one row
        columns = Mathf.Max(1, targetColumns);
        rows = Mathf.Max(1, targetRows);

        // Swap if image is tall (more rows than columns)
        if (aspect < 1.0f && maintainAspectRatio)
        {
            columns = Mathf.Max(1, targetRows);
            rows = Mathf.Max(1, targetColumns);
        }
    }

    void AutoSizeToScreen()
    {
        Camera cam = Camera.main;
        float vertExtent = cam.orthographicSize;
        float horizExtent = vertExtent * cam.aspect;

        float boardHeight = 2f * vertExtent * 0.85f;
        float boardWidth = 2f * horizExtent * 0.85f;

        float tileSize = Mathf.Min(boardWidth / columns, boardHeight / rows);
        tileWidth = tileSize;
        tileHeight = tileSize;
        tileSize = Mathf.Min(tileSize, 5f); // Avoid board overflow when piece count is low (set number to whatever max size you want)
    }

    void GenerateEdgeTypes()
    {
        int segments = curveResolution + 1;   // e.g., 9 points for 8 segments
        float maxOffset = 0.15f; // maximum jagged height (will be scaled by edge length)
        /*if (rows > 1)
        {
            hEdgeOffsets = new int[columns, rows - 1];
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows - 1; y++)
                {
                    hEdgeOffsets[x, y] = Random.value > 0.5f ? 1 : -1;
                }
            }
        }
        else hEdgeOffsets = new int[0, 0];*/

        if (rows > 1)
        {
            hEdgeOffsets = new List<float>[columns, rows - 1];
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows - 1; y++)
                {
                    List<float> offsets = new List<float>(segments);
                    offsets.Add(0f);
                    for (int i = 0; i < segments - 1; i++)
                        offsets.Add(Random.Range(-maxOffset, maxOffset));
                    offsets.Add(0f);
                    hEdgeOffsets[x, y] = offsets;
                }
            }
        }
        else hEdgeOffsets = new List<float>[0, 0];

        /*if (columns > 1)
        {
            vEdgeOffsets = new int[columns - 1, rows];
            for (int x = 0; x < columns - 1; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    vEdgeOffsets[x, y] = Random.value > 0.5f ? 1 : -1;
                }
            }
        }
        else vEdgeOffsets = new int[0, 0];*/

        if (columns > 1)
        {
            vEdgeOffsets = new List<float>[columns - 1, rows];
            for (int x = 0; x < columns - 1; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    List<float> offsets = new List<float>(segments);
                    offsets.Add(0f);
                    for (int i = 1; i < segments - 1; i++)
                        offsets.Add(Random.Range(-maxOffset, maxOffset));
                    offsets.Add(0f);
                    vEdgeOffsets[x, y] = offsets;
                }
            }

        }
        else vEdgeOffsets = new List<float>[0, 0];
    }

    void GenerateBoard()
    {
        // Calculate how big each piece should be in world space
        // (Can either use the fixed tileWidth/tileHeight or derive from image size

        // Use the individual piece size (already calculated by AutoSizeToScreen or manual)
        float pieceWidth = tileWidth;
        float pieceHeight = tileHeight;

        // Total board size Z(used only for centering)
        float boardWidth = columns * pieceWidth;
        float boardHeight = rows * pieceHeight;
        Vector3 offset = new Vector3(-boardWidth / 2f, -boardHeight / 2f, 0);

        // Loop through grid
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                // Instantiate a new tile from the prefab, as a child of this board
                GameObject tileObj = Instantiate(tilePrefab, transform);
                // name the piece for clarity
                tileObj.name = $"Tile_{x}_{y}";

                // Get the JigsawTile script
                JigsawTile tile = tileObj.GetComponent<JigsawTile>();

                // Pass all the required info
                tile.sourceImage = puzzleImage;
                tile.tileIndexX = x;
                tile.tileIndexY = y;
                tile.gridSizeX = columns;
                tile.gridSizeY = rows;
                tile.tileWidth = pieceWidth;
                tile.tileHeight = pieceHeight;
                tile.hEdgeOffsets = hEdgeOffsets;
                tile.vEdgeOffsets = vEdgeOffsets;

                // Generate its mesh (this also applies the material)
                tile.GenerateMesh();

                // Position the tile in world space
                // (place the bottom-left corner of the corner of the board at (0,0) for easy math)

                tileObj.transform.position = new Vector3(x * pieceWidth, y * pieceHeight, 0) + offset;
            }
        }
    }
}
