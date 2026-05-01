using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Zero_Gravity : MonoBehaviour
{
    public float driftSpeed = 2f;
    public float leftPullStrength = 0.5f;
    public float maxSpeed = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Start with a random drifting direction
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        rb.linearVelocity = randomDir * driftSpeed;

        // No gravity for true zero-G feel
        rb.gravityScale = 0f;
    }

    void FixedUpdate()
    {
        // Constant pull to the left
        rb.linearVelocity += Vector2.left * leftPullStrength * Time.fixedDeltaTime;

        // Clamp speed so it doesn't go crazy
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the collision normal (direction of the surface hit)
        Vector2 normal = collision.contacts[0].normal;

        // Reflect velocity like a bounce
        Vector2 newVelocity = Vector2.Reflect(rb.linearVelocity, normal);

        // Reapply leftward bias so it keeps drifting left overall
        newVelocity += Vector2.left * leftPullStrength;

        rb.linearVelocity = newVelocity;
    }
}