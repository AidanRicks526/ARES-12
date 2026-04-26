using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.Rendering;

public class JigsawInteraction : JigsawTile //IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Snapping")]
    //public float snapDistance = 0.5f;     // How close to correct position before it snaps
    public bool lockWhenSnapped = true;   // Disable dragging after snapping

    //private Vector3 offset;
    //private bool isDragging = false;
    private bool isSnapped = false;
    public Vector3 CorrectPosition => correctPosition;
    public bool IsSnapped => isSnapped;

    void Start()
    {
        // The board already set correctPosition before shuffling
        // If this piece was not shuffled, fallback to current position
        if (correctPosition == Vector3.zero)
            correctPosition = transform.position;
        
    }

    /*public void OnPointerDown(PointerEventData eventData)
    {

        if (isSnapped) return;

        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;

        // bring the piece to front while dragging
        GetComponent<SortingGroup>().sortingOrder = 1;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
            transform.position = GetMouseWorldPosition() + offset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        // restore sorting order
        GetComponent<SortingGroup>().sortingOrder = 0;

        // Check if we are within snap distance
        if (Vector3.Distance(transform.position, correctPosition) <= snapDistance)
        {
            transform.position = correctPosition;
            isSnapped = true;

            if (lockWhenSnapped)
                this.enabled = false; // Disable this script so it can no longer be dragged
        }
    }*/

    public void LockPiece()
    {
        isSnapped = true;
        this.enabled = false;
    }

    /*private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }*/
}
