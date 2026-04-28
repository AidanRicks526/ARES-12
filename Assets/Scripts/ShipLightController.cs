using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShipLightController : MonoBehaviour
{
    public Light2D[] shipLights;

    public float dimIntensity = 0.1f;
    public float fullIntensity = 1f;

    void Start()
    {
        SetLightsDim();
    }

    public void SetLightsDim()
    {
        SetIntensity(dimIntensity);
        Debug.Log("Ship lights DIM");
    }

    public void SetLightsFull()
    {
        SetIntensity(fullIntensity);
        Debug.Log("Ship lights FULL");
    }

    void SetIntensity(float value)
    {
        foreach (var light in shipLights)
        {
            if (light != null)
                light.intensity = value;
        }
    }
}