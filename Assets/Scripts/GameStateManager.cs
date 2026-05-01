using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public bool lightsOn = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("GameStateManager ACTIVE | lightsOn = " + lightsOn);
    }

    public void EnableLights()
    {
        lightsOn = true;
        Debug.Log("LIGHTS ENABLED GLOBALLY");
    }
}