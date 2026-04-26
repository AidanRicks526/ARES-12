using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleDragBridge : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Camera puzzleCamera;          // Reference to the PuzzleCamera
    public float snapDistance = 0.5f;    // Same as in JigsawInteraction (you can sync)
    public bool lockWhenSnapped = true;

    private JigsawInteraction currentPiece;
    private Vector3 offset;
    private bool isDragging = false;

    void Start()
    {
        // Ensure the Raw Image receives events
        if (!GetComponent<CanvasGroup>())
            gameObject.AddComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Convert the screen position to a world point for the puzzle camera
        Vector3 worldPoint = ScreenPointToWorld(eventData.position);

        // Find the puzzle piece under the cursor
        Collider2D hit = Physics2D.OverlapPoint(worldPoint);
        if (hit != null)
        {
            JigsawInteraction piece = hit.GetComponent<JigsawInteraction>();
            if (piece != null && !piece.IsSnapped)  // you may need to expose IsSnapped
            {
                currentPiece = piece;
                offset = currentPiece.transform.position - worldPoint;
                isDragging = true;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && currentPiece != null)
        {
            Vector3 worldPoint = ScreenPointToWorld(eventData.position);
            currentPiece.transform.position = worldPoint + offset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging && currentPiece != null)
        {
            Vector3 worldPoint = ScreenPointToWorld(eventData.position);
            // Check snap distance
            if (Vector3.Distance(currentPiece.transform.position, currentPiece.CorrectPosition) <= snapDistance)
            {
                currentPiece.transform.position = currentPiece.CorrectPosition;
                if (lockWhenSnapped)
                    currentPiece.LockPiece();
            }
        }
        currentPiece = null;
        isDragging = false;
    }

    Vector3 ScreenPointToWorld(Vector2 screenPos)
    {
        // Raw Image's RectTransform gives us the position on the UI
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GetComponent<RectTransform>(), screenPos, null, out Vector2 localPoint);

        // Normalize local point to 0..1 range
        Vector2 normalized = Rect.PointToNormalized(GetComponent<RectTransform>().rect, localPoint);

        // Convert to world coordinates for the puzzle camera
        Vector3 worldPos = puzzleCamera.ViewportToWorldPoint(
            new Vector3(normalized.x, normalized.y, puzzleCamera.nearClipPlane));
        worldPos.z = 0;
        return worldPos;
    }
}