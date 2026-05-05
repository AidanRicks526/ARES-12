using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class IsoMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 15f;

    float baseScaleX;

    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 currentVelocity;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool moving;

    [Header("Outfit State")]
    public bool isInSuitMode = false;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        baseScaleX = Mathf.Abs(transform.localScale.x);
    }

    // Called by PlayerInput
    public void OnMove(InputValue value)
    {
        if (LockerInteract.IsUIOpenGlobal)
        {
            input = Vector2.zero;
            return;
        }

        input = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        if (LockerInteract.IsUIOpenGlobal)
        {
            currentVelocity = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = input.normalized;
        Vector2 targetVelocity = direction * moveSpeed;

        if (input.magnitude > 0)
        {
            currentVelocity = Vector2.Lerp(
                currentVelocity,
                targetVelocity,
                acceleration * Time.fixedDeltaTime
            );
        }
        else
        {
            currentVelocity = Vector2.Lerp(
                currentVelocity,
                Vector2.zero,
                deceleration * Time.fixedDeltaTime
            );
        }

        rb.linearVelocity = currentVelocity;
    }

    void Update()
    {
        if (LockerInteract.IsUIOpenGlobal)
        {
            input = Vector2.zero;
            moving = false;

            if (animator != null && !isInSuitMode)
                animator.SetBool("Running", false);

            return;
        }

        moving = input.magnitude > 0.1f;

        if (!isInSuitMode && animator != null)
        {
            animator.SetBool("Running", moving);
        }

        if (!isInSuitMode && input.x != 0)
        {
            transform.localScale = new Vector3(
                baseScaleX * Mathf.Sign(input.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

    public void SetSuitMode(Sprite suitSprite)
    {
        isInSuitMode = true;

        if (animator != null)
        {
            animator.enabled = false;
        }

        if (spriteRenderer != null && suitSprite != null)
        {
            spriteRenderer.sprite = suitSprite;
        }
    }
}