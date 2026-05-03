using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    [Header("Countdown Settings")]
    public float startTime = 300f; // 5 minutes
    public float currentTime;

    public bool isRunning = false;
    private bool initialized = false;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;

            Debug.Log("TIMER REACHED ZERO");

            OnTimerFinished();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!initialized && scene.name == "Hallway")
        {
            StartTimer();
            initialized = true;
        }
    }

    public void StartTimer()
    {
        currentTime = startTime;
        isRunning = true;

        Debug.Log("Countdown Timer Started: " + GetFormattedTime());
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    void OnTimerFinished()
    {
        // 🔥 Hook for game over / AI reaction
        Debug.Log("Time is up!");

        // Example:
        // FindObjectOfType<ShipAI>()?.PlayDialogue(failureDialogue);
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public float GetTimeRemaining()
    {
        return currentTime;
    }
}