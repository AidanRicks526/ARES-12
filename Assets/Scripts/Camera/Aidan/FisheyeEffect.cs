using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FisheyeEffect : MonoBehaviour
{
    public Material fisheyeMaterial;

    // This built-in Unity function is called after the camera finishes rendering.
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Check if the material is assigned in the Inspector.
        if (fisheyeMaterial != null)
        {
            // Apply the material's shader to the image.
            Graphics.Blit(source, destination, fisheyeMaterial);
        }
        else
        {
            // If no material, just pass the image through unchanged.
            Graphics.Blit(source, destination);
        }
    }
}