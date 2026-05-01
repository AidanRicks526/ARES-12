using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Key = scene name | Value = set of ItemData asset names that are done
    private Dictionary<string, HashSet<string>> collectedItems = new();
    private Dictionary<string, HashSet<string>> completedPuzzles = new();
    private Dictionary<string, HashSet<string>> completedMiniGames = new();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ─── Helpers ──────────────────────────────────────────────────────────────

    private string ActiveScene => SceneManager.GetActiveScene().name;

    private static void Register(Dictionary<string, HashSet<string>> dict, string scene, string id)
    {
        if (!dict.ContainsKey(scene)) dict[scene] = new HashSet<string>();
        dict[scene].Add(id);
    }

    private static bool Contains(Dictionary<string, HashSet<string>> dict, string scene, string id)
        => dict.TryGetValue(scene, out var set) && set.Contains(id);

    // ─── Collected Items ──────────────────────────────────────────────────────

    public void RegisterCollected(ItemData id)
        => Register(collectedItems, ActiveScene, id.name);

    public bool IsCollected(ItemData id)
        => Contains(collectedItems, ActiveScene, id.name);

    // ─── Puzzles ──────────────────────────────────────────────────────────────

    public void RegisterPuzzleComplete(ItemData id)
        => Register(completedPuzzles, ActiveScene, id.name);

    public bool IsPuzzleComplete(ItemData id)
        => Contains(completedPuzzles, ActiveScene, id.name);

    // ─── Mini-Games ───────────────────────────────────────────────────────────

    public void RegisterMiniGameComplete(ItemData id)
        => Register(completedMiniGames, ActiveScene, id.name);

    public bool IsMiniGameComplete(ItemData id)
        => Contains(completedMiniGames, ActiveScene, id.name);

    // ─── Save / Load ──────────────────────────────────────────────────────────

    public void SaveProgress()
    {
        PlayerPrefs.SetString("collectedItems", Serialize(collectedItems));
        PlayerPrefs.SetString("completedPuzzles", Serialize(completedPuzzles));
        PlayerPrefs.SetString("completedMiniGames", Serialize(completedMiniGames));
        PlayerPrefs.Save();
    }

    public void LoadProgress()
    {
        if (PlayerPrefs.HasKey("collectedItems"))
            collectedItems = Deserialize(PlayerPrefs.GetString("collectedItems"));
        if (PlayerPrefs.HasKey("completedPuzzles"))
            completedPuzzles = Deserialize(PlayerPrefs.GetString("completedPuzzles"));
        if (PlayerPrefs.HasKey("completedMiniGames"))
            completedMiniGames = Deserialize(PlayerPrefs.GetString("completedMiniGames"));
    }

    public void ResetAllProgress()
    {
        collectedItems.Clear();
        completedPuzzles.Clear();
        completedMiniGames.Clear();
        PlayerPrefs.DeleteAll();
    }

    // ─── Serialization ────────────────────────────────────────────────────────

    [System.Serializable] private class Entry { public string key; public List<string> values; }
    [System.Serializable] private class SaveData { public List<Entry> entries = new(); }

    private static string Serialize(Dictionary<string, HashSet<string>> dict)
    {
        var data = new SaveData();
        foreach (var kv in dict)
            data.entries.Add(new Entry { key = kv.Key, values = new List<string>(kv.Value) });
        return JsonUtility.ToJson(data);
    }

    private static Dictionary<string, HashSet<string>> Deserialize(string json)
    {
        var result = new Dictionary<string, HashSet<string>>();
        var data = JsonUtility.FromJson<SaveData>(json);
        if (data == null) return result;
        foreach (var e in data.entries)
            result[e.key] = new HashSet<string>(e.values);
        return result;
    }
}
