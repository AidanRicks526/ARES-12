using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] private float smoothSpeed = 5f;

    [Header("Zoom (closeness)")]
    [SerializeField] private float zoomSize = 5f;      // smaller = closer
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 10f;

    private Camera cam;
    private Transform player;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = zoomSize;

        // Find the object with the "Player" tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("No GameObject with tag 'Player' found in the scene!");
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Smooth follow
        Vector3 targetPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }

    // Call this method to change zoom at runtime (e.g., from a UI slider)
    public void SetZoom(float newZoom)
    {
        zoomSize = Mathf.Clamp(newZoom, minZoom, maxZoom);
        cam.orthographicSize = zoomSize;
    }
}