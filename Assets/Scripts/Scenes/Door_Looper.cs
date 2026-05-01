using UnityEngine;

public class Door_Looper : MonoBehaviour
{
    public Transform[] doors;
    public float speed = 5f;
    public float spacing = 10f;

    private float leftBound;
    private float rightBound;

    void Start()
    {
        // establish stable bounds once
        float center = 0f;

        leftBound = center - spacing;
        rightBound = center + spacing;
    }

    void Update()
    {
        float input = 0f;

        bool a = Input.GetKey(KeyCode.A);
        bool d = Input.GetKey(KeyCode.D);

        if (a && !d)
            input = 1f;
        else if (d && !a)
            input = -1f;
        else
            input = 0f;

        float move = input * speed * Time.deltaTime;

        foreach (Transform door in doors)
        {
            door.position += Vector3.right * move;

            if (door.position.x < leftBound)
            {
                door.position += Vector3.right * spacing * doors.Length;
            }

            if (door.position.x > rightBound)
            {
                door.position -= Vector3.right * spacing * doors.Length;
            }
        }
    }
}