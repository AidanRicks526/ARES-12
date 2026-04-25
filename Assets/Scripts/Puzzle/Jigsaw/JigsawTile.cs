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
    //[HideInInspector] public int[,] hEdges; // reference to board's horizontal array
    //[HideInInspector] public int[,] vEdges; // reference to board's vertical array
    [HideInInspector] public List<float>[,] hEdgeOffsets;
    [HideInInspector] public List<float>[,] vEdgeOffsets;
    [SerializeField] private float indentStrength = 0.15f;

    Vector3[] SampleEdge(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int resolution)
    {
        Vector3[] points = new Vector3[resolution + 1];
        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            points[i] = BezierCurve.CalculateCubicBezierPoint(t, p0, p1, p2, p3);
        }

        return points;
    }

    public struct EdgeControlPoints
    {
        public Vector3 p0, p1, p2, p3;
    }

    EdgeControlPoints GetEdgePoints(Vector3 start, Vector3 end, int edgeType)
    {
        Vector3 dir = (end - start).normalized;
        Vector3 perp = new Vector3(-dir.y, dir.x, 0); // 90 degree rotation

        float length = Vector3.Distance(start, end);
        // Strenght controls how far the tab/blank bulges
        float strength = length * indentStrength; // you can tweak indentStrength for deeper/shallower indendts

        if (edgeType == 0) // flat
        {
            return new EdgeControlPoints
            {
                p0 = start,
                p1 = start,
                p2 = end,
                p3 = end
            };
        }

        // For tab (+1): bulges outward (positive perp)
        // For blank (-1): bulges inward (negative perp)
        float sign = (edgeType > 0) ? 1f : -1f;

        // Asymmetric bulge: the peak is at 60% of the edge, not in the middle.
        // This creates a classic jigsaw tab shape.
        float peakT = 0.6f; // The position of the farthest point

        // Control point 1 pulls the curve outward near 30% along the edge
        float t1 = peakT * 0.5f;
        // Control point 2 pushes the curve back inward after the peak
        float t2 = peakT + (1f - peakT) * 0.5f;

        Vector3 p1 = start + dir * length * t1 + perp * sign * strength * 0.6f;
        Vector3 p2 = start + dir * length * t2 + perp * sign * strength * 1.2f;
        // The peak vertex will naturally appear near peakT*length, not at a control point.

        // For a sharper tab, we can also add a third control point? Actually cubic Bézier
        // can only have 4 points, but we can still make the bulge asymmetric.

        // Alternative simpler asymmetric: just offset the control points differently:
        // p1 further along, p2 further back.
        // We'll use the above for a nice jagged look.

        return new EdgeControlPoints
        {
            p0 = start,
            p1 = p1,
            p2 = p2,
            p3 = end
        };
    }

    public void GenerateMesh()
    {
        //Corner positions
        Vector3 cornerBL = new Vector3(0, 0, 0);
        Vector3 cornerBR = new Vector3(tileWidth, 0, 0);
        Vector3 cornerTR = new Vector3(tileWidth, tileHeight, 0);
        Vector3 cornerTL = new Vector3(0, tileHeight, 0);

        /*Vector3 topP1 = cornerTL;            // on left side, no offset
        Vector3 topP2 = cornerTR;            // on right side, no offset
        Vector3 bottomP1 = cornerBL;
        Vector3 bottomP2 = cornerBR;
        Vector3 rightP1 = cornerBR;
        Vector3 rightP2 = cornerTR;
        Vector3 leftP1 = cornerBL;
        Vector3 leftP2 = cornerTL;*/

        // Determine edge types, with borders flat (0)
        /*int topType = 0;
        if (tileIndexY < gridSizeY - 1) topType = hEdges[tileIndexX, tileIndexY];

        int bottomType = 0;
        if (tileIndexY > 0) bottomType = -hEdges[tileIndexX, tileIndexY - 1]; // mirrored

        int rightType = 0;
        if (tileIndexX < gridSizeX - 1) rightType = vEdges[tileIndexX, tileIndexY];

        int leftType = 0;
        if (tileIndexX > 0) leftType = -vEdges[tileIndexX - 1, tileIndexY];*/ // mirrored

        // Get control points for each edge
        EdgeControlPoints topCtrl = GetEdgePoints(cornerTL, cornerTR, topType);
        EdgeControlPoints bottomCtrl = GetEdgePoints(cornerBL, cornerBR, bottomType);
        EdgeControlPoints rightCtrl = GetEdgePoints(cornerBR, cornerTR, rightType);
        EdgeControlPoints leftCtrl = GetEdgePoints(cornerBL, cornerTL, leftType);

        Vector3[] topEdge = SampleEdge(topCtrl.p0, topCtrl.p1, topCtrl.p2, topCtrl.p3, curveResolution);
        Vector3[] bottomEdge = SampleEdge(bottomCtrl.p0, bottomCtrl.p1, bottomCtrl.p2, bottomCtrl.p3, curveResolution);
        Vector3[] rightEdge = SampleEdge(rightCtrl.p0, rightCtrl.p1, rightCtrl.p2, rightCtrl.p3, curveResolution);
        Vector3[] leftEdge = SampleEdge(leftCtrl.p0, leftCtrl.p1, leftCtrl.p2, leftCtrl.p3, curveResolution);

        int gridSize = curveResolution + 1;
        Vector3[] vertices = new Vector3[gridSize * gridSize];

        for (int j = 0; j < gridSize; j++)
        {
            for (int i = 0; i < gridSize; i++)
            {
                // Edge points to interpolate
                Vector3 topPoint = topEdge[i]; // horizontal index
                Vector3 bottomPoint = bottomEdge[i];
                Vector3 leftPoint = leftEdge[j]; // vertical index
                Vector3 rightPoint = rightEdge[j];

                // Horizontal interpolation (left -> right)
                Vector3 hLerp = Vector3.Lerp(leftPoint, rightPoint, i / (float)curveResolution);
                // Vertical interpolation (bottom -> top)
                Vector3 vLerp = Vector3.Lerp(bottomPoint, topPoint, j / (float)curveResolution);

                // Final smooth position (averaging the two interpolations)
                vertices[j * gridSize + i] = (hLerp + vLerp) * 0.5f;
            }
        }

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int j = 0;j < gridSize;j++)
        {
            float vCell = tileIndexY + j / (float)curveResolution; // vertical position inside whole image
            for (int i = 0;i < gridSize;i++)
            {
                float uCell = tileIndexX + i / (float)curveResolution;
                // Normalize to 0-1 range
                uvs[j * gridSize + i] = new Vector2(uCell / gridSizeX, vCell / gridSizeY);
            }
            Debug.Log("UV range: min=(" + uvs[0].x + "," + uvs[0].y + ")  max=(" + uvs[uvs.Length - 1].x + "," + uvs[uvs.Length - 1].y + ")");
        }

        int[] triangles = new int[(curveResolution * curveResolution) * 6];
        int triIndex = 0;

        for (int j =0;j < curveResolution;j++)
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

        Mesh mesh = new Mesh();
        mesh.name = "JigsawTileMesh";
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;

        //Material setup
        Material mat = new Material(Shader.Find("Unlit/Texture"));
        //Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.mainTexture = sourceImage;
        GetComponent<MeshRenderer>().material = mat;
        Debug.Log("Mesh assigned. Material mainTex: " + mat.mainTexture);
    }
}