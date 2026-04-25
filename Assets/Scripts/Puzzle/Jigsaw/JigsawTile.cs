using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class JigsawTile : MonoBehaviour
{
    public Texture2D sourceImage; // The full puzzle image
    public int tileIndexX, tileIndexY; // This piece's position in the grid
    public int gridSizeX, gridSizeY; // The total number of rows and columns
    public float tileWidth, tileHeight; // The size of a single piece 
    public int curveResolution = 10; // How many points make up each curve (higher = smoother)



}
