using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class IsoMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 15f;

    [Header("Audio Scene Settings")]
    public string disableAudioScene = "";

    private float baseScaleX;

    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 currentVelocity;

    private Animator animator;

    private bool moving;
    private bool animationDisabled;

    // =========================
    // NEW: RUN AUDIO STATE
    // =========================
    private bool running;

    [Header("Audio")]
    public AudioSource movementAudioSource; // 👈 footsteps / running loop

    [Header("Scene Settings")]
    public string disableAnimationScene = "";

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        baseScaleX = Mathf.Abs(transform.localScale.x);
    }

    void Start()
    {
        CheckScene();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckScene();
    }

    void CheckScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // Animation toggle
        if (!string.IsNullOrEmpty(disableAnimationScene) &&
            currentScene == disableAnimationScene)
        {
            animationDisabled = true;
        }
        else
        {
            animationDisabled = false;
        }

        // AUDIO toggle (NEW)
        if (!string.IsNullOrEmpty(disableAudioScene) &&
            currentScene == disableAudioScene)
        {
            if (movementAudioSource != null)
                movementAudioSource.Stop();
        }
    }

    public void OnMove(InputValue value)
    {
        if (LockerInteract.IsUIOpenGlobal)
        {
            input = Vector2.zero;
            SetRunning(false);
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

            SetRunning(false);
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

            SetRunning(true);
        }
        else
        {
            currentVelocity = Vector2.Lerp(
                currentVelocity,
                Vector2.zero,
                deceleration * Time.fixedDeltaTime
            );

            SetRunning(false);
        }

        rb.linearVelocity = currentVelocity;
    }

    void Update()
    {
        if (LockerInteract.IsUIOpenGlobal)
        {
            input = Vector2.zero;
            moving = false;

            SetRunning(false);

            if (!animationDisabled && animator != null)
                animator.SetBool("Running", false);

            return;
        }

        moving = input.magnitude > 0.1f;

        if (!animationDisabled && animator != null)
        {
            animator.SetBool("Running", moving);
        }

        if (input.x != 0)
        {
            transform.localScale = new Vector3(
                baseScaleX * Mathf.Sign(input.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

    // AUDIO CONTROL (RUN STATE)

    void SetRunning(bool value)
    {
        if (running == value) return;

        running = value;

        if (movementAudioSource == null) return;

        // BLOCK AUDIO IN DISABLED SCENE
        if (!string.IsNullOrEmpty(disableAudioScene) &&
            SceneManager.GetActiveScene().name == disableAudioScene)
        {
            movementAudioSource.Stop();
            return;
        }

        if (running)
        {
            if (!movementAudioSource.isPlaying)
                movementAudioSource.Play();
        }
        else
        {
            movementAudioSource.Pause();
        }
    }
}