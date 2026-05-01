using UnityEngine;

public class Door_Looper : MonoBehaviour
{
    public Transform[] doors;
    public float speed = 5f;
    public float spacing = 10f;

    private Vector3[] baseLocalPositions;

    void Start()
    {
        baseLocalPositions = new Vector3[doors.Length];

        // restore saved state OR initialize
        for (int i = 0; i < doors.Length; i++)
        {
            if (DoorState.savedLocalPositions != null &&
                DoorState.savedLocalPositions.Length == doors.Length)
            {
                doors[i].localPosition = DoorState.savedLocalPositions[i];
            }

            baseLocalPositions[i] = doors[i].localPosition;
        }
    }

    void Update()
    {
        float input = 0f;

        bool a = Input.GetKey(KeyCode.A);
        bool d = Input.GetKey(KeyCode.D);

        if (a && !d) input = 1f;
        else if (d && !a) input = -1f;

        float move = input * speed * Time.deltaTime;

        for (int i = 0; i < doors.Length; i++)
        {
            Vector3 pos = doors[i].localPosition;
            pos.x += move;

            float minX = baseLocalPositions[i].x - spacing;
            float maxX = baseLocalPositions[i].x + spacing;

            if (pos.x < minX)
                pos.x += spacing * doors.Length;

            if (pos.x > maxX)
                pos.x -= spacing * doors.Length;

            doors[i].localPosition = pos;
        }
    }

    public void SaveDoorPositions()
    {
        DoorState.savedLocalPositions = new Vector3[doors.Length];

        for (int i = 0; i < doors.Length; i++)
        {
            DoorState.savedLocalPositions[i] = doors[i].localPosition;
        }
    }
}