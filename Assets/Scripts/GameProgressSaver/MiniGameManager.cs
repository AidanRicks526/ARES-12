using UnityEngine;

/// <summary>
/// Attach to any mini-game controller.
/// Drag a ItemData asset into the 'id' field in the Inspector.
/// Call CompleteMiniGame() from your mini-game logic on success.
/// </summary>
public class MiniGameManager : MonoBehaviour
{
    public ItemData id;

    private void Start()
    {
        if (GameManager.Instance.IsMiniGameComplete(id))
            gameObject.SetActive(false);
    }

    public void CompleteMiniGame()
    {
        GameManager.Instance.RegisterMiniGameComplete(id);
        gameObject.SetActive(false);
    }
}
