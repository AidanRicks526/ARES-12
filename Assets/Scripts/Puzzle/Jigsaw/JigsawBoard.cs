//using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JigsawBoard : MonoBehaviour
{
    [Header("Puzzle Configuration")]
    public GameObject tilePrefab; // Drag JigsawTile prefab here
    public Texture2D puzzleImage; // The full puzzle texture
    public bool randomizePieceCount = false;
    public int totalPieces = 12;  // Number of pieces
    public int minPieces = 12;
    public int maxPieces = 25;
    public bool autoCalculate = true; // Manual override
    public int manualColumns = 4, manualRows = 4;// Manual override
    public bool maintainAspectRatio = true; // keep pieces square-ish

    [HideInInspector] public Vector3 boardCenter;
    [HideInInspector] public float boardWorldWidth;
    [HideInInspector] public float boardWorldHeight;

    private int columns, rows;

    // Shared offset lists so adjacent pieces perfectly interlock
    private List<float>[,] hEdgeOffsets; // [columns, rows - 1]
    private List<float>[,] vEdgeOffsets; // [columns - 1, rows]

    private List<GameObject> allPieces = new List<GameObject>();

    [Header("Sizes")]
    public float tileWidth = 1.0f;       // World-space width of each piece
    public float tileHeight = 1.0f;      // World-space height of each piece
    public int curveResolution = 8;      // Must match the tile's curveResolution

    /*void Start()
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
        ShufflePieces();
    }*/
    void OnEnable()
    {
        if (randomizePieceCount)
            totalPieces = Random.Range(minPieces, maxPieces + 1);

        //Clear any previously generated pieces
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        allPieces.Clear();

        if (autoCalculate)
            CalculateGrid();
        else
        {
            columns = manualColumns;
            rows = manualRows;
        }

        Debug.Log($"JigsawBoard generating: {columns} columns x {rows} rows");


        AutoSizeToScreen();

        Debug.Log($"Tile size: {tileWidth} x {tileHeight}");

        GenerateEdgeTypes();
        GenerateBoard();
        ShufflePieces();

        Debug.Log($"Generated {allPieces.Count} pieces");
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

        // Make sure it doesn't exceed the total piece count 
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

    [Range(0.1f, 1f)]
    public float screenFillFraction = 0.85f; // % of the camera viewport the board may occupy

    void AutoSizeToScreen()
    {
        Camera cam = Camera.main;
        float vertExtent = cam.orthographicSize;
        float horizExtent = vertExtent * cam.aspect;

        // Available area on screen (with margin)
        float maxBoardWidth  = 2f * horizExtent * screenFillFraction;
        float maxBoardHeight = 2f * vertExtent  * screenFillFraction;

        // Image aspect (width / height)
        float imgAspect = (float)puzzleImage.width / puzzleImage.height;
        float availAspect = maxBoardWidth / maxBoardHeight;

        float boardWidth, boardHeight;
        if (imgAspect > availAspect)
        {
            // Image is wider than the viewport area — width-limited
            boardWidth = maxBoardWidth;
            boardHeight = boardWidth / imgAspect;
        }
        else
        {
            // Image is taller (or matches) — height-limited
            boardHeight = maxBoardHeight;
            boardWidth = boardHeight * imgAspect;
        }

        // Tiles fill the board exactly. They will not be perfectly square
        // unless columns/rows happens to match the image's aspect ratio.
        tileWidth = boardWidth / columns;
        tileHeight = boardHeight / rows;
    }

    void GenerateEdgeTypes()
    {
        int segments = curveResolution + 1;   // e.g., 9 points for 8 segments
        float maxOffset = 0.15f; // maximum jagged height (scaled by edge length)

        // Generate Horizontal Seam Offsets
        if (rows > 1)
        {
            hEdgeOffsets = new List<float>[columns, rows - 1];
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows - 1; y++)
                {
                    List<float> offsets = new List<float>(segments);
                    offsets.Add(0f); // Start corner flat
                    for (int i = 1; i < segments - 1; i++)
                        offsets.Add(Random.Range(-maxOffset, maxOffset));
                    offsets.Add(0f); // End corner flat

                    hEdgeOffsets[x, y] = offsets;
                }
            }
        }
        else hEdgeOffsets = new List<float>[0, 0];

        // Generate Vertical Seam Offsets
        if (columns > 1)
        {
            vEdgeOffsets = new List<float>[columns - 1, rows];
            for (int x = 0; x < columns - 1; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    List<float> offsets = new List<float>(segments);
                    offsets.Add(0f); // Start corner flat
                    for (int i = 1; i < segments - 1; i++)
                        offsets.Add(Random.Range(-maxOffset, maxOffset));
                    offsets.Add(0f); // End corner flat

                    vEdgeOffsets[x, y] = offsets;
                }
            }
        }
        else vEdgeOffsets = new List<float>[0, 0];
        Debug.Log($"puzzleImage null? {puzzleImage == null}");
    }

    void DrawBoardBorder()
    {
        GameObject borderObj = new GameObject("BoardBorder");
        borderObj.transform.parent = transform;

        LineRenderer lr = borderObj.AddComponent<LineRenderer>();
        lr.positionCount = 5;
        lr.loop = false;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.material.color = Color.white;

        float hw = boardWorldWidth / 2f;
        float hh = boardWorldHeight / 2f;
        Vector3 c = boardCenter;

        lr.SetPosition(0, new Vector3(c.x - hw, c.y - hh, 0));
        lr.SetPosition(1, new Vector3(c.x + hw, c.y - hh, 0));
        lr.SetPosition(2, new Vector3(c.x + hw, c.y + hh, 0));
        lr.SetPosition(3, new Vector3(c.x - hw, c.y + hh, 0));
        lr.SetPosition(4, new Vector3(c.x - hw, c.y - hh, 0));
    }

    void GenerateBoard()
    {
        float pieceWidth = tileWidth;
        float pieceHeight = tileHeight;

        // Total board size (used only for centering)
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
                allPieces.Add(tileObj);
                tileObj.name = $"Tile_{x}_{y}";

                // Get the JigsawTile script
                JigsawTile tile = tileObj.GetComponent<JigsawTile>();

                // Pass all the required procedural parameters
                tile.sourceImage = puzzleImage;
                tile.tileIndexX = x;
                tile.tileIndexY = y;
                tile.gridSizeX = columns;
                tile.gridSizeY = rows;
                tile.tileWidth = pieceWidth;
                tile.tileHeight = pieceHeight;
                tile.hEdgeOffsets = hEdgeOffsets;
                tile.vEdgeOffsets = vEdgeOffsets;

                // Generate its mesh mapping
                tile.GenerateMesh();

                // Position the tile in world space
                tileObj.transform.position = new Vector3(x * pieceWidth, y * pieceHeight, 0) + offset;

                // Store board bounds
                //boardCenter = Vector3.zero; // world position of JigsawBoard
                boardCenter = transform.position; // world position of JigsawBoard
                boardWorldWidth = boardWidth;
                boardWorldHeight = boardHeight;

                DrawBoardBorder();
            }
        }
    }

    void ShufflePieces()
    {
        // Get the camera bounds so pieces stay visible
        Camera cam = Camera.main;
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        // Keep pieces within 80% of the screen, centered
        /*float xMin = -camWidth / 2f * 0.8f;
        float xMax = camWidth / 2f * 0.8f;
        float yMin = -camHeight / 2f * 0.8f;
        float yMax = camHeight / 2f * 0.8f;*/
        float xMin = -camWidth / 2f + tileWidth / 2f;
        float xMax = camWidth / 2f - tileWidth / 2f;
        float yMin = -camHeight / 2f + tileHeight / 2f;
        float yMax = camHeight / 2f - tileHeight / 2f;

        foreach (GameObject pieceObj in allPieces)
        {
            JigsawTile tile = pieceObj.GetComponent<JigsawTile>();

            // 1. Store the correct position (where the board just placed it)
            tile.correctPosition = pieceObj.transform.position;

            // 2. Assign a random position inside the safe zone
            float randX = Random.Range(xMin, xMax);
            float randY = Random.Range(yMin, yMax);
            pieceObj.transform.position = new Vector3(randX, randY, 0);

            // 3. (Optional) Random rotation – keep angles like 0, 90, 180, 270 for clarity
            // int rotations = Random.Range(0, 4);
            // pieceObj.transform.rotation = Quaternion.Euler(0, 0, rotations * 90f);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boardCenter, new Vector3(boardWorldWidth, boardWorldHeight, 0));
    }
}