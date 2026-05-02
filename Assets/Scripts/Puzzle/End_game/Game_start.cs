using UnityEngine;

public class Game_start : MonoBehaviour
{
    [Header("Scene Objects")]
    public GameObject mazeObject;      // contains edge collider maze
    public GameObject playerObject;    // your square

    private bool triggered;

    void Update()
    {
        if (triggered) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            triggered = true;

            mazeObject.SetActive(true);
            playerObject.SetActive(true);

            Destroy(gameObject); // remove trigger after starting
        }
    }
}