using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Loop Settings")]
    public float loopPoint = 20f;   // distance before teleport
    public float resetPoint = -20f; // where it teleports back to

    void Update()
    {
        float move = 0f;

        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);

        // only move if ONE direction is pressed
        if (left && !right)
        {
            move = moveSpeed; // background goes right
        }
        else if (right && !left)
        {
            move = -moveSpeed; // background goes left
        }

        transform.position += new Vector3(move * Time.deltaTime, 0f, 0f);

        // LOOP / TELEPORT logic
        if (transform.position.x >= loopPoint)
        {
            transform.position = new Vector3(resetPoint, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= resetPoint)
        {
            transform.position = new Vector3(loopPoint, transform.position.y, transform.position.z);
        }
    }
}