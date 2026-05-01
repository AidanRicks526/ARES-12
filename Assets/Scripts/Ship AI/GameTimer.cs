using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    public float startTime = 600f;
    public float currentTime;

    void Awake()
    {
        Instance = this;
        currentTime = startTime;
    }

    void Update()
    {
        currentTime = Mathf.Max(0f, currentTime - Time.deltaTime);
    }

    public bool IsTimeUp()
    {
        return currentTime <= 0f;
    }
}