using UnityEngine;

/// <summary>
/// Attach to any collectible (memory shard, pick-up, etc.)
/// Drag a ItemData asset into the 'id' field in the Inspector.
/// </summary>
public class Collectible : MonoBehaviour
{
    public ItemData id;

    private void Start()
    {
        if (id == null)
        {
            Debug.LogWarning($"Collectible '{name}' has no ItemData assigned.", this);
            return;
        }
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.IsCollected(id))
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Collect();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Collect();
    }

    private void Collect()
    {
        GameManager.Instance.RegisterCollected(id);
        gameObject.SetActive(false);
    }
}
