using UnityEngine;

public class Persistence : MonoBehaviour
{
    public static Persistence instance;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null); // detach from any parent first
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
