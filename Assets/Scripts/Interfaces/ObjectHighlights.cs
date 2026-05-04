using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObjectHighlights : MonoBehaviour
{
    [Header("Settings")]
    public float highlightScale = 1.1f;
    public float detectionRadius = 3f;
    public string playerTag = "Player";

    [Header("Pulse Settings")]
    public float pulseSpeed = 5f;
    public float pulseAmount = 0.05f;

    [Header("Material")]
    public Material highlightMaterial; // 👈 assign your WhiteSprite material here

    private SpriteRenderer original;
    private SpriteRenderer highlight;
    private Transform highlightTransform;
    private Transform player;

    void Start()
    {
        original = GetComponent<SpriteRenderer>();

        // Find player
        GameObject p = GameObject.FindGameObjectWithTag(playerTag);
        if (p != null)
            player = p.transform;

        // Create highlight object
        GameObject clone = new GameObject("Highlight");
        clone.transform.SetParent(transform);
        clone.transform.localPosition = Vector3.zero;
        clone.transform.localRotation = Quaternion.identity;

        highlightTransform = clone.transform;

        highlight = clone.AddComponent<SpriteRenderer>();

        // Copy sprite + sorting
        highlight.sprite = original.sprite;
        highlight.sortingLayerID = original.sortingLayerID;
        highlight.sortingOrder = original.sortingOrder - 1;

        // 👇 THIS is the magic now
        highlight.material = highlightMaterial;

        // Optional softness
        highlight.color = new Color(1f, 1f, 1f, 0.6f);

        // Base size
        highlightTransform.localScale = Vector3.one * highlightScale;

        highlight.enabled = false;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= detectionRadius)
        {
            EnableHighlight();
            PulseEffect();
        }
        else
        {
            DisableHighlight();
        }

        // Keep sprite synced if it changes
        if (highlight.sprite != original.sprite)
            highlight.sprite = original.sprite;
    }

    void EnableHighlight()
    {
        if (!highlight.enabled)
            highlight.enabled = true;
    }

    void DisableHighlight()
    {
        if (highlight.enabled)
            highlight.enabled = false;
    }

    void PulseEffect()
    {
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        highlightTransform.localScale = Vector3.one * highlightScale * pulse;
    }
}