using UnityEngine;

public class CatBoxJump : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpXTarget = 5f; // X coordinate where the jump starts
    public float jumpHeight = 1.5f;
    public float jumpDuration = 0.5f;

    private bool isJumping = false;
    private float jumpTimer = 0f;
    private Vector2 jumpStartPos;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    void FixedUpdate()
    {
        if (!isJumping)
        {
            // move right
            rb.linearVelocity = Vector2.right * moveSpeed;

            // check if reached the box X position
            if (transform.position.x >= jumpXTarget)
            {
                StartJump();
            }
        }
        else
        {
            UpdateJump();
        }
    }

    void StartJump()
    {
        isJumping = true;
        jumpStartPos = transform.position;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false; // stop physics during jump
    }

    void UpdateJump()
    {
        jumpTimer += Time.fixedDeltaTime;
        float percent = jumpTimer / jumpDuration;

        // parabolic arc math
        float yOffset = Mathf.Sin(percent * Mathf.PI) * jumpHeight;
        float xOffset = percent * 1.0f; // slight move forward into box

        transform.position = new Vector3(
            jumpStartPos.x + xOffset,
            jumpStartPos.y + yOffset,
            0
        );

        // shrink the cat as it "goes into" the box
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, percent);

        if (percent >= 1f)
        {
            Destroy(gameObject);
        }
    }
}