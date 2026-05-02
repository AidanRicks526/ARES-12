using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager instance;

    private static bool _loadFromDoor;

    private GameObject _player;
    private DoorTriggerInteraction.DoorToSpawnAt _doorToSpawnTo;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static void SwapSceneFromDoorUse(SceneField myScene, DoorTriggerInteraction.DoorToSpawnAt doorToSpawnAt)
    {

        _loadFromDoor = true;
        FindFirstObjectByType<CircularHallway>()?.SaveOrientation(); instance._doorToSpawnTo = doorToSpawnAt;
        instance.StartCoroutine(instance.LoadSceneRoutine(myScene));
    }

    private IEnumerator LoadSceneRoutine(SceneField myScene)
    {
        SceneFadeManager.instance.StartFadeOut();

        while (SceneFadeManager.instance.IsFadingOut)
            yield return null;

        SceneManager.LoadScene(myScene.SceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneFadeManager.instance.StartFadeIn();

        if (!_loadFromDoor) return;

        // ALWAYS re-fetch player after scene load
        _player = GameObject.FindGameObjectWithTag("Player");

        if (_player == null)
        {
            Debug.LogError("Player not found in scene!");
            return;
        }

        DoorTriggerInteraction targetDoor = FindDoor(_doorToSpawnTo);

        if (targetDoor == null)
        {
            Debug.LogError("Target door not found!");
            return;
        }

        // DIRECT SPAWN USING ACTUAL SCENE POSITION
        _player.transform.position = targetDoor.GetSpawnPosition();

        _loadFromDoor = false;
    }

    private DoorTriggerInteraction FindDoor(DoorTriggerInteraction.DoorToSpawnAt door)
    {
        DoorTriggerInteraction[] doors =
            FindObjectsByType<DoorTriggerInteraction>(FindObjectsSortMode.None);

        foreach (var d in doors)
        {
            if (d.CurrentDoorPosition == door)
                return d;
        }

        return null;
    }
}