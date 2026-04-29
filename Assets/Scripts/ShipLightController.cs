using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShipLightController : MonoBehaviour
{
    [Header("Ship Lights")]
    public Light2D[] shipLights;

    [Header("Darkness Overlay (UI)")]
    public CanvasGroup darknessOverlay;

    [Header("Intensity Settings")]
    public float dimIntensity = 0.1f;
    public float fullIntensity = 1f;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    void Start()
    {
        SetLightsDim();
        SetOverlayInstant(true);
    }

    public void SetLightsDim()
    {
        SetIntensity(dimIntensity);
        FadeOverlay(true);
    }

    public void SetLightsFull()
    {
        SetIntensity(fullIntensity);
        FadeOverlay(false);
    }

    void SetIntensity(float value)
    {
        if (shipLights == null) return;

        foreach (var light in shipLights)
        {
            if (light != null)
                light.intensity = value;
        }
    }

    void SetOverlayInstant(bool enabled)
    {
        if (darknessOverlay == null) return;

        darknessOverlay.alpha = enabled ? 1f : 0f;
        darknessOverlay.blocksRaycasts = enabled;
        darknessOverlay.interactable = enabled;
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

        darknessOverlay.blocksRaycasts = fadeIn;
        darknessOverlay.interactable = fadeIn;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = t / fadeDuration;

            darknessOverlay.alpha = Mathf.Lerp(start, end, normalized);

            yield return null;
        }

        darknessOverlay.alpha = end;
    }
}