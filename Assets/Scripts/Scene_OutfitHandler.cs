using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_OutfitHandler : MonoBehaviour
{
    [Header("Outfit Data")]
    public ItemData astronautSuitItem;
    public Sprite astronautSuitSprite;

    [Header("Scene Settings (optional)")]
    public string requiredSceneName = ""; // leave empty to apply in all scenes

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Delay check slightly to ensure Inventory singleton exists
        Invoke(nameof(CheckAndApplyOutfit), 0.1f);
    }

    void CheckAndApplyOutfit()
    {
        // Scene gate (optional)
        if (!string.IsNullOrEmpty(requiredSceneName) &&
            SceneManager.GetActiveScene().name != requiredSceneName)
        {
            return;
        }

        // Safety checks
        if (Inventory.Instance == null)
        {
            Debug.LogWarning("[Outfit] Inventory instance not found yet.");
            return;
        }

        if (astronautSuitItem == null)
        {
            Debug.LogWarning("[Outfit] Astronaut suit ItemData not assigned.");
            return;
        }

        // Check ownership
        if (!Inventory.Instance.HasItem(astronautSuitItem))
        {
            Debug.Log("[Outfit] Player does NOT have suit.");
            return;
        }

        ApplySuit();
    }

    void ApplySuit()
    {
        Debug.Log("[Outfit] Applying astronaut suit...");

        // Disable animation override
        if (animator != null)
        {
            animator.enabled = false;
        }

        // Swap sprite
        if (spriteRenderer != null && astronautSuitSprite != null)
        {
            spriteRenderer.sprite = astronautSuitSprite;
        }
        else
        {
            Debug.LogWarning("[Outfit] Missing SpriteRenderer or suit sprite.");
        }
    }
}