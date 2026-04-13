using UnityEngine;

public static class Initializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        Debug.Log("Initializer executed before scene load.");
        Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("PERSISTOBJECTS")));


    }

}
