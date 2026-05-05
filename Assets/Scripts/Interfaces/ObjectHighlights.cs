using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObjectHighlights : MonoBehaviour
{
    [Header("Settings")]
    public float highlightScale = 1.05f;
    public float detectionRadius = 1f;
    public string playerTag = "Player";

    [Header("Pulse Settings")]
    public float pulseSpeed = 3f;
    public float pulseAmount = 0.05f;

    [Header("Material")]
    public Material highlightMaterial;

    private SpriteRenderer original;
    private SpriteRenderer highlight;
    private Transform highlightTransform;
    private Transform player;

    void Start()
    {
        original = GetComponent<SpriteRenderer>();

        GameObject p = GameObject.FindGameObjectWithTag(playerTag);
        if (p != null) player = p.transform;

        GameObject clone = new GameObject("Highlight_Effect");
        clone.transform.SetParent(transform);

        // Ensure it sits exactly on top of the parent
        clone.transform.localPosition = Vector3.zero;
        clone.transform.localRotation = Quaternion.identity;
        clone.transform.localScale = Vector3.one;

        highlightTransform = clone.transform;
        highlight = clone.AddComponent<SpriteRenderer>();

        highlight.sprite = original.sprite;
        highlight.sortingLayerID = original.sortingLayerID;
        highlight.sortingOrder = original.sortingOrder - 1;
        highlight.material = highlightMaterial;
        highlight.color = new Color(1f, 1f, 1f, 0.6f);

        highlight.enabled = false;
    }

    void Update()
    {
        if (player == null || highlight == null) return;

        // 1. ABSOLUTE SYNC
        // This copies the visual state of the sprite regardless of how it's flipped
        highlight.sprite = original.sprite;
        highlight.flipX = original.flipX;
        highlight.flipY = original.flipY;

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
    }

    void EnableHighlight() { if (!highlight.enabled) highlight.enabled = true; }
    void DisableHighlight() { if (highlight.enabled) highlight.enabled = false; }

    void PulseEffect()
    {
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;

        // We multiply by highlightScale to create the 'outline' look
        // We use Vector3.one because it's a child; it already inherits the parent's 0.227 scale
        highlightTransform.localScale = Vector3.one * (pulse * highlightScale);
    }
}