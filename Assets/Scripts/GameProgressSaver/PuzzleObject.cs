using UnityEngine;

/// <summary>
/// Attach to any puzzle GameObject (jigsaw, symbol sequence, etc.)
/// Drag a ItemData asset into the 'id' field in the Inspector.
/// Call CompletePuzzle() from your puzzle logic when the player solves it.
/// </summary>
public class PuzzleObject : MonoBehaviour
{
    public ItemData id;

    private void Start()
    {
        if (id == null)
        {
            Debug.LogWarning($"PuzzleObject '{name}' has no ItemData assigned.", this);
            return;
        }
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.IsPuzzleComplete(id))
            gameObject.SetActive(false);
    }

    public void CompletePuzzle()
    {
        GameManager.Instance.RegisterPuzzleComplete(id);
        gameObject.SetActive(false);
    }
}
