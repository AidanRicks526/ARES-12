using UnityEngine;

public class TestFisheye : MonoBehaviour
{
    public Material mat;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Debug.Log("OnRenderImage called");
        if (mat != null)
            Graphics.Blit(src, dest, mat);
        else
            Graphics.Blit(src, dest);
    }
}