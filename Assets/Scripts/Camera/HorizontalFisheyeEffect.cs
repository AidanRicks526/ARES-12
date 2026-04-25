using UnityEngine;

[RequireComponent(typeof(Camera))]
public class HorizontalFisheyeEffect : MonoBehaviour
{
    public Material fisheyeMaterial;
    [Range(-1f, 1f)] public float strength = 0.3f;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (fisheyeMaterial != null)
        {
            fisheyeMaterial.SetFloat("_Strength", strength);
            Graphics.Blit(src, dest, fisheyeMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}