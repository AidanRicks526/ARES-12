using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KittyCat : MonoBehaviour
{
    public Transform door;
    public float moveSpeed = 3f;
    public float avoidRadius = 4f;
    public float avoidStrength = 4f;

    private Transform player;
    private Rigidbody2D rb;

    private SpriteRenderer sprite;
    private bool isExiting = false;
    private float fadeSpeed = 3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>(); // needed for fading
        rb.freezeRotation = true;
        rb.gravityScale = 0f;
    }

    void Start()
    {
        // search for player at start
        FindPlayer();
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }





    void FixedUpdate()
    {
        // check distance to door
        float distToDoor = Vector2.Distance(transform.position, door.position);

        if (distToDoor < 0.2f || isExiting)
        {
            StartExit();
            return;
        }

        if (player == null) { FindPlayer(); return; }
        if (LockerInteract.IsUIOpenGlobal) { rb.linearVelocity = Vector2.zero; return; }

        Vector2 pos = transform.position;
        Vector2 toDoor = ((Vector2)door.position - pos).normalized;
        Vector2 toPlayer = (Vector2)player.position - pos;
        float distToPlayer = toPlayer.magnitude;

        Vector2 moveDir = toDoor;

        if (distToPlayer < avoidRadius && Vector2.Dot(toDoor, toPlayer.normalized) > 0)
        {
            Vector2 away = toPlayer.normalized;
            Vector2 tangentA = new Vector2(-away.y, away.x);
            Vector2 tangentB = new Vector2(away.y, -away.x);
            Vector2 bestTangent = Vector2.Dot(tangentA, toDoor) > Vector2.Dot(tangentB, toDoor) ? tangentA : tangentB;

            float weight = 1f - (distToPlayer / avoidRadius);
            moveDir = Vector2.Lerp(toDoor, bestTangent, weight * avoidStrength).normalized;
        }

        rb.linearVelocity = moveDir * moveSpeed;
    }

    void StartExit()
    {
        isExiting = true;
        rb.linearVelocity = Vector2.zero; // stop moving so it doesn't jitter

        // reduce alpha over time
        Color c = sprite.color;
        c.a -= fadeSpeed * Time.fixedDeltaTime;
        sprite.color = c;

        if (c.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}