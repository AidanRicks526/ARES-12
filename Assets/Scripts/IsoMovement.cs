using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class IsoMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 15f;

    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 currentVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Called by PlayerInput component
    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        Vector2 isoDirection = new Vector2(
            input.x - input.y,
            (input.x + input.y) / 2f
        ).normalized;

        Vector2 targetVelocity = isoDirection * moveSpeed;

        if (input.magnitude > 0)
            currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        else
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);

        rb.linearVelocity = currentVelocity;
    }
}