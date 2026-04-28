using UnityEngine;

public class SceneLightingController : MonoBehaviour
{
    public GameObject darknessGroup;
    public GameObject visibilityGroup;
    public GameObject shipLights;

    void Start()
    {
        ApplyState();
    }

    void ApplyState()
    {
        bool lightsOn = GameStateManager.Instance != null && GameStateManager.Instance.lightsOn;

        Debug.Log("LIGHT STATE = " + lightsOn);

        // Ship lights
        if (shipLights != null)
            shipLights.SetActive(lightsOn);

        // UI groups
        if (darknessGroup != null)
            darknessGroup.SetActive(!lightsOn);

        if (visibilityGroup != null)
            visibilityGroup.SetActive(!lightsOn);
    }
}