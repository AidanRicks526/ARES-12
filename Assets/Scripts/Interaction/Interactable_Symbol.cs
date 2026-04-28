using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Interaction_Symbol : MonoBehaviour
{
    [Header("Setup")]
    public string playerTag = "Player";

    [Header("Fade Settings")]
    public float fadeSpeed = 5f;

    [Header("Linked Object")]
    public GameObject objectToWatch; // drag this in inspector

    private SpriteRenderer spriteRenderer;
    private float targetAlpha = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;

        // start invisible
        SetAlpha(0f);
    }

    void Update()
    {
        // if watched object is gone → destroy this
        if (objectToWatch == null)
        {
            Destroy(gameObject);
            return;
        }

        // smooth fade
        float currentAlpha = spriteRenderer.color.a;
        float newAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
        SetAlpha(newAlpha);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            targetAlpha = 1f; // fade in
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            targetAlpha = 0f; // fade out
        }
    }

    private void SetAlpha(float alpha)
    {
        Color c = spriteRenderer.color;
        c.a = alpha;
        spriteRenderer.color = c;
    }
}