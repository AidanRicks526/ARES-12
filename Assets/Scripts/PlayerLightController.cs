using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLightController : MonoBehaviour
{
    [Header("Player Light Object")]
    public GameObject playerLightObject;

    [Header("Scene Settings")]
    public string disableLightInScene; // 👈 set this in Inspector

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

    public void ApplyState()
    {
        if (playerLightObject == null)
        {
            Debug.LogError("PlayerLightController: playerLightObject NOT assigned");
            return;
        }

        // 👇 Check current scene
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == disableLightInScene)
        {
            playerLightObject.SetActive(false);
            Debug.Log("PlayerLight forced OFF in scene: " + currentScene);
            return;
        }

        if (GameStateManager.Instance == null)
        {
            Debug.LogWarning("PlayerLightController: No GameStateManager");
            return;
        }

        bool lightsOn = GameStateManager.Instance.lightsOn;

        playerLightObject.SetActive(!lightsOn);
        Debug.Log("PlayerLight active: " + (!lightsOn));
    }
}