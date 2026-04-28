using UnityEngine;

public class PlayerVisibilityController : MonoBehaviour
{
    private RectTransform circleUI;
    private Camera cam;

    void Start()
    {
        Debug.Log("VISIBILITY SCRIPT STARTED");
        cam = Camera.main;

        GameObject circle = GameObject.Find("VisibilityCircle");

        if (circle != null)
        {
            circleUI = circle.GetComponent<RectTransform>();
            Debug.Log("Circle found!");
        }
        else
        {
            Debug.LogError("VisibilityCircle NOT found!");
        }
    }

    void LateUpdate()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.lightsOn)
            return;

        circleUI.position = cam.WorldToScreenPoint(transform.position);
    }
}