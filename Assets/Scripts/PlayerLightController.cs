using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLightController : MonoBehaviour
{
    [Header("Player Light Object")]
    public GameObject playerLightObject;

    void Start()
    {
        ApplyState();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyState();
    }

    // 🔥 PUBLIC so other scripts (Puzzle) can force update
    public void ApplyState()
    {
        if (GameStateManager.Instance == null)
        {
            Debug.LogWarning("PlayerLightController: No GameStateManager");
            return;
        }

        bool lightsOn = GameStateManager.Instance.lightsOn;

        if (playerLightObject != null)
        {
            playerLightObject.SetActive(!lightsOn);
            Debug.Log("PlayerLight active: " + (!lightsOn));
        }
        else
        {
            Debug.LogError("PlayerLightController: playerLightObject NOT assigned");
        }
    }
}