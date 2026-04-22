using UnityEngine;

public class UpDownLoop : MonoBehaviour
{
    public float amplitude = 1f;   // how high it moves
    public float speed = 2f;       // how fast it moves

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = startPos + new Vector3(0f, offset, 0f);
    }
}