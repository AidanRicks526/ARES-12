using UnityEngine;

public class Win_Zone : MonoBehaviour
{
    public Minigame_FadeManager fadeManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Square"))
        {
            fadeManager.ShowWin();
        }
    }
}