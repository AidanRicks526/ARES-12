using UnityEngine;
using System.Collections;

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
    //private MeshRenderer meshRenderer;
    

    /*void Start()
    {
        if (correctPosition == Vector3.zero)
            correctPosition = transform.position;
    }*/
    void Awake()
    {
        //meshRenderer = GetComponent<MeshRenderer>();
        SetZ(0f);
    }

    void OnMouseDown()
    {
        if (isSnapped) return;
        dragOffset = transform.position - GetMouseWorldPos();
        isDragging = true;
        SetZ(-1f);
        //GetComponent<Renderer>().sortingOrder = 10;
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;
        SetZ(-1f);
        transform.position = GetMouseWorldPos() + dragOffset;
    }

    void OnMouseUp()
    {
        if (!isDragging) return;
        isDragging = false;
        SetZ(0f);
        //GetComponent<Renderer>().sortingOrder = 0;

        if (Vector3.Distance(transform.position, correctPosition) <= snapDistance)
        {
            transform.position = correctPosition;
            LockPiece();
        }
    }

    public void LockPiece()
    {
        isSnapped = true;
        SetZ(1f);
        if (lockWhenSnapped)
            this.enabled = false;

        FindFirstObjectByType<JigsawPuzzleManager>().CheckCompletion();
    }

    private void SetZ(float z)
    {
        Vector3 p = transform.position;
        transform.position = new Vector3(p.x, p.y, z);
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = Mathf.Abs(Camera.main.transform.position.z);
        return Camera.main.ScreenToWorldPoint(mouse);
    }
}