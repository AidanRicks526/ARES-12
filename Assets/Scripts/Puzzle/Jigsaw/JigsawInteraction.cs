using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class JigsawInteraction : JigsawTile
{
    [Header("Snapping")]
    public float snapDistance = 0.3f;
    public bool lockWhenSnapped = true;

    private bool isSnapped = false;
    private bool isDragging = false;
    private Vector3 dragOffset;

    public Vector3 CorrectPosition => correctPosition;
    public bool IsSnapped => isSnapped;

    void Start()
    {
        if (correctPosition == Vector3.zero)
            correctPosition = transform.position;
    }

    void OnMouseDown()
    {
        if (isSnapped) return;
        dragOffset = transform.position - GetMouseWorldPos();
        isDragging = true;
        GetComponent<Renderer>().sortingOrder = 10;
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;
        transform.position = GetMouseWorldPos() + dragOffset;
    }

    void OnMouseUp()
    {
        if (!isDragging) return;
        isDragging = false;
        GetComponent<Renderer>().sortingOrder = 0;

        if (Vector3.Distance(transform.position, correctPosition) <= snapDistance)
        {
            transform.position = correctPosition;
            LockPiece();
        }
    }

    public void LockPiece()
    {
        isSnapped = true;
        if (lockWhenSnapped)
            this.enabled = false;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = Mathf.Abs(Camera.main.transform.position.z);
        return Camera.main.ScreenToWorldPoint(mouse);
    }
}