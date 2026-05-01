using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShipLightController : MonoBehaviour
{
    [Header("Global Light (auto-found if empty)")]
    public Light2D globalLight;

    [Header("Darkness Overlay")]
    public CanvasGroup darknessOverlay;

    [Header("Intensity Settings")]
    public float dimIntensity = 0.1f;
    public float fullIntensity = 1f;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    void Awake()
    {
        AutoFindGlobalLight();
    }

    void Start()
    {
        ApplyStateFromGameManager();
    }

    void AutoFindGlobalLight()
    {
        if (globalLight != null) return;

        Light2D[] lights = FindObjectsOfType<Light2D>();

        foreach (var light in lights)
        {
            if (light.lightType == Light2D.LightType.Global)
            {
                globalLight = light;
                Debug.Log("Auto-found Global Light in scene: " + light.name);
                return;
            }
        }

        Debug.LogError("No Global Light2D found in scene!");
    }

    void ApplyStateFromGameManager()
    {
        if (GameStateManager.Instance == null)
        {
            Debug.LogError("No GameStateManager found!");
            return;
        }

        if (GameStateManager.Instance.lightsOn)
        {
            Debug.Log("Scene load → LIGHTS ON");
            SetLightsImmediate(fullIntensity);
            SetOverlayInstant(false);
        }
        else
        {
            Debug.Log("Scene load → LIGHTS OFF");
            SetLightsImmediate(dimIntensity);
            SetOverlayInstant(true);
        }
    }

    public void SetLightsFull()
    {
        SetLightsImmediate(fullIntensity);
        FadeOverlay(false);
    }

    public void SetLightsDim()
    {
        SetLightsImmediate(dimIntensity);
        FadeOverlay(true);
    }

    void SetLightsImmediate(float value)
    {
        if (globalLight != null)
        {
            globalLight.intensity = value;
            Debug.Log("Set Global Light intensity to: " + value);
        }
        else
        {
            Debug.LogError("Global Light missing!");
        }
    }

    void SetOverlayInstant(bool enabled)
    {
        if (darknessOverlay == null) return;

        darknessOverlay.alpha = enabled ? 1f : 0f;
        darknessOverlay.blocksRaycasts = enabled;
    }

    void FadeOverlay(bool fadeIn)
    {
        if (darknessOverlay == null) return;

        StopAllCoroutines();
        StartCoroutine(FadeRoutine(fadeIn));
    }

    IEnumerator FadeRoutine(bool fadeIn)
    {
        float start = darknessOverlay.alpha;
        float end = fadeIn ? 1f : 0f;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            darknessOverlay.alpha = Mathf.Lerp(start, end, t / fadeDuration);
            yield return null;
        }

        darknessOverlay.alpha = end;
    }
}