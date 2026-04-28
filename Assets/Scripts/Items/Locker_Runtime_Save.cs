using System.Collections.Generic;


public static class Locker_Runtime_Save
{
    private static HashSet<string> unlockedLockers = new HashSet<string>();

    public static bool IsUnlocked(string id)
    {
        return unlockedLockers.Contains(id);
    }

    public static void Unlock(string id)
    {
        unlockedLockers.Add(id);
    }

    public static void ResetAll()
    {
        unlockedLockers.Clear();
    }
}
