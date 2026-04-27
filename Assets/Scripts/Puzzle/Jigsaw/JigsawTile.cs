using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class JigsawTile : MonoBehaviour
{
    public Texture2D sourceImage; // The full puzzle image
    public int tileIndexX, tileIndexY; // This piece's position in the grid
    public int gridSizeX, gridSizeY; // The total number of rows and columns
    public float tileWidth = 1.0f; // The size of a single piece width
    public float tileHeight = 1.0f; // The size of a single piece height
    public int curveResolution = 8; // How many points make up each curve (higher = smoother)

    [HideInInspector] public List<float>[,] hEdgeOffsets;
    [HideInInspector] public List<float>[,] vEdgeOffsets;

    [HideInInspector] public Vector3 correctPosition;

    [SerializeField] private float indentStrength = 0.15f;

    Vector3[] GenerateJaggedEdge(Vector3 start, Vector3 end, List<float> offsets)
    {
        int count = offsets.Count;
        Vector3[] points = new Vector3[count];
        Vector3 dir = (end - start).normalized;
        Vector3 perp = new Vector3(-dir.y, dir.x, 0); // Perpendicular direction for the bumps
        float length = Vector3.Distance(start, end);

        for (int i = 0; i < count; i++)
        {
            float t = i / (float)(count - 1);
            Vector3 basePoint = start + dir * length * t;
            // Apply the shared offset to the perpendicular vector
            points[i] = basePoint + perp * (offsets[i] * length * indentStrength);
        }
        return points;
    }

    public void GenerateMesh()
    {
        // Static Corner positions
        Vector3 cornerBL = new Vector3(0, 0, 0);
        Vector3 cornerBR = new Vector3(tileWidth, 0, 0);
        Vector3 cornerTR = new Vector3(tileWidth, tileHeight, 0);
        Vector3 cornerTL = new Vector3(0, tileHeight, 0);

        // ---------- Generate jagged edges from offset lists ----------
        int gridSize = curveResolution + 1;
        List<float> flatOffsets = new List<float>(gridSize);
        for (int i = 0; i < gridSize; i++) flatOffsets.Add(0f);

        // Top edge
        Vector3[] topEdge;
        if (tileIndexY < gridSizeY - 1)
        {
            List<float> offsets = hEdgeOffsets[tileIndexX, tileIndexY];
            topEdge = GenerateJaggedEdge(cornerTL, cornerTR, offsets);
        }
        else
            topEdge = GenerateJaggedEdge(cornerTL, cornerTR, flatOffsets);

        // Bottom edge (No reversal needed! Both tile borders share the exact same spatial vector and offset logic)
        Vector3[] bottomEdge;
        if (tileIndexY > 0)
        {
            List<float> offsets = hEdgeOffsets[tileIndexX, tileIndexY - 1];
            bottomEdge = GenerateJaggedEdge(cornerBL, cornerBR, offsets);
        }
        else
            bottomEdge = GenerateJaggedEdge(cornerBL, cornerBR, flatOffsets);

        // Right edge
        Vector3[] rightEdge;
        if (tileIndexX < gridSizeX - 1)
        {
            List<float> offsets = vEdgeOffsets[tileIndexX, tileIndexY];
            rightEdge = GenerateJaggedEdge(cornerBR, cornerTR, offsets);
        }
        else
            rightEdge = GenerateJaggedEdge(cornerBR, cornerTR, flatOffsets);

        // Left edge (No reversal needed! Identical spatial flow as the right edge)
        Vector3[] leftEdge;
        if (tileIndexX > 0)
        {
            List<float> offsets = vEdgeOffsets[tileIndexX - 1, tileIndexY];
            leftEdge = GenerateJaggedEdge(cornerBL, cornerTL, offsets);
        }
        else
            leftEdge = GenerateJaggedEdge(cornerBL, cornerTL, flatOffsets);

        // ---------- Coons Patch Surface Interpolation ----------
        Vector3[] vertices = new Vector3[gridSize * gridSize];

        for (int j = 0; j < gridSize; j++)
        {
            float tv = j / (float)curveResolution; // 0 = bottom, 1 = top
            for (int i = 0; i < gridSize; i++)
            {
                float tu = i / (float)curveResolution; // 0 = left, 1 = right

                Vector3 bottom = bottomEdge[i];
                Vector3 top = topEdge[i];
                Vector3 left = leftEdge[j];
                Vector3 right = rightEdge[j];

                // Bilinear blend: weight each edge by how close we are to it
                // FIXED BUG: We must subtract the static *corners* to avoid double-counting space.
                Vector3 v = bottom * (1 - tv) + top * tv       // vertical contribution
                          + left * (1 - tu) + right * tu      // horizontal contribution
                          - (cornerBL * (1 - tv) * (1 - tu)   // subtract static corners
                           + cornerBR * (1 - tv) * tu
                           + cornerTL * tv * (1 - tu)
                           + cornerTR * tv * tu);

                vertices[j * gridSize + i] = v;
            }
        }

        // ---------- UV Mapping ----------
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int idx = 0; idx < vertices.Length; idx++)
        {
            // vertices are in local space: x in [0, tileWidth], y in [0, tileHeight]
            // map to the tile's portion of the full image
            float u = (tileIndexX * tileWidth + vertices[idx].x) / (gridSizeX * tileWidth);
            float v = (tileIndexY * tileHeight + vertices[idx].y) / (gridSizeY * tileHeight);
            uvs[idx] = new Vector2(u, v);
        }

        // ---------- Triangle Generation ----------
        int[] triangles = new int[(curveResolution * curveResolution) * 6];
        int triIndex = 0;

        for (int j = 0; j < curveResolution; j++)
        {
            for (int i = 0; i < curveResolution; i++)
            {
                int bl = j * gridSize + i;             // bottom-left
                int br = j * gridSize + (i + 1);       // bottom-right
                int tl = (j + 1) * gridSize + i;       // top-left
                int tr = (j + 1) * gridSize + (i + 1); // top-right

                // First triangle (bl -> tl -> br)
                triangles[triIndex++] = bl;
                triangles[triIndex++] = tl;
                triangles[triIndex++] = br;

                // Second triangle (br -> tl -> tr)
                triangles[triIndex++] = br;
                triangles[triIndex++] = tl;
                triangles[triIndex++] = tr;
            }
        }

        // ---------- Mesh Finalization ----------
        Mesh mesh = new Mesh();
        mesh.name = "JigsawTileMesh";
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;

        // Material setup
        Material mat = new Material(Shader.Find("Unlit/Texture"));
        mat.mainTexture = sourceImage;
        GetComponent<MeshRenderer>().material = mat;


        // Simple box collider for click detection
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col == null) col = gameObject.AddComponent<BoxCollider2D>();
        col.size = new Vector2(tileWidth, tileHeight);
        col.offset = new Vector2(tileWidth / 2f, tileHeight / 2f);
        col.isTrigger = true;

        //Debug.Log($"Tile_{tileIndexX}_{tileIndexY} mesh assigned. Renderer enabled: {GetComponent<MeshRenderer>().enabled}, Material: {GetComponent<MeshRenderer>().material.mainTexture}");
        Debug.Log($"Tile_{tileIndexX}_{tileIndexY} sourceImage null? {sourceImage == null}, mat tex null? {mat.mainTexture == null}");
    }
}