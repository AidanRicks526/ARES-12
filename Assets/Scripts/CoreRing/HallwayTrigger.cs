using UnityEngine;

public class HallwayTrigger : MonoBehaviour
{
    public bool isLeftTrigger; // true = left side, false = right side

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Get the Rigidbody2D to know movement direction
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        float horizontalVelocity = rb.linearVelocity.x; // use rb.velocity.x for older Unity

        // Right trigger (collider_R) – only recycle when moving right
        if (!isLeftTrigger && horizontalVelocity > 0)
        {
            CircularHallway manager = FindObjectOfType<CircularHallway>();
            manager?.MoveLeftmostToRight();
        }
        // Left trigger (collider_L) – only recycle when moving left
        else if (isLeftTrigger && horizontalVelocity < 0)
        {
            CircularHallway manager = FindObjectOfType<CircularHallway>();
            manager?.MoveRightmostToLeft();
        }
        // If player is moving opposite direction or standing still, do nothing
    }
}