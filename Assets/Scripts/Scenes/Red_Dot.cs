using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Red_Dot : MonoBehaviour
{
    [Header("Pulse Settings")]
    public float speed = 2f;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;

    public float minAlpha = 0.5f;
    public float maxAlpha = 1f;

    private Image img;

    void Awake()
    {
        img = GetComponent<Image>();
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;

        // Scale pulse
        float scale = Mathf.Lerp(minScale, maxScale, t);
        transform.localScale = new Vector3(scale, scale, 1f);

        // Alpha pulse (glow feel)
        Color c = img.color;
        c.a = Mathf.Lerp(minAlpha, maxAlpha, t);
        img.color = c;
    }
}