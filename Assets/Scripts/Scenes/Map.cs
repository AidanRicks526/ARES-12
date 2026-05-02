using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform mapPanel;   // Parent of RawImage
    public GameObject closeButton;

    [Header("Positions")]
    public Vector2 hiddenPos;
    public Vector2 shownPos;

    [Header("Settings")]
    public float slideSpeed = 6f;

    private bool isOpen = false;
    private Coroutine slideRoutine;

    void Start()
    {
        mapPanel.anchoredPosition = hiddenPos;
        closeButton.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }

    public void ToggleMap()
    {
        isOpen = !isOpen;

        if (slideRoutine != null)
            StopCoroutine(slideRoutine);

        slideRoutine = StartCoroutine(Slide(isOpen ? shownPos : hiddenPos));

        closeButton.SetActive(isOpen);
    }

    public void CloseMap()
    {
        if (!isOpen) return;

        isOpen = false;

        if (slideRoutine != null)
            StopCoroutine(slideRoutine);

        slideRoutine = StartCoroutine(Slide(hiddenPos));

        closeButton.SetActive(false);
    }

    IEnumerator Slide(Vector2 target)
    {
        Vector2 start = mapPanel.anchoredPosition;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * slideSpeed;
            mapPanel.anchoredPosition = Vector2.Lerp(start, target, t);
            yield return null;
        }

        mapPanel.anchoredPosition = target;
    }
}