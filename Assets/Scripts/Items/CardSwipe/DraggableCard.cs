using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Vector2 startPosition;

    public RectTransform sensorRect;     // Assign in Inspector
    public float swipeThreshold = 0.5f;  // Fraction of sensor width to cross

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Horizontal movement only, lock Y
        Vector2 newPos = rectTransform.anchoredPosition;
        newPos.x += eventData.delta.x / transform.lossyScale.x;

        // Clamp between start position and a little past the sensor
        float maxX = sensorRect.anchoredPosition.x + 50f;
        newPos.x = Mathf.Clamp(newPos.x, startPosition.x, maxX);
        newPos.y = startPosition.y;

        rectTransform.anchoredPosition = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Check if card has moved far enough to the right
        if (rectTransform.anchoredPosition.x >= sensorRect.anchoredPosition.x * swipeThreshold)
        {
            CardSwipeManager.Instance.OnSwipeSuccess();
        }
        else
        {
            // Return to start
            rectTransform.anchoredPosition = startPosition;
        }
    }
}