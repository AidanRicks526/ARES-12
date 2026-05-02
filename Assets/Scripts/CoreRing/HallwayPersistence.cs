using UnityEngine;

public class HallwayPersistence : MonoBehaviour
{
    public static HallwayPersistence Instance;

    [HideInInspector] public int savedOffset = 0;
    [HideInInspector] public bool hasSavedState = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}