using UnityEngine;
using System.Collections;

public class Minigame_EffectRunner : MonoBehaviour
{
    public Minigame_fade_manager fadeManager;

    [Header("Outcome")]
    public bool isWin;

    [Header("Animation")]
    public Animator animator;
    public float delayBeforeTrigger = 1f;

    private bool played;

    public void PlayEffect()
    {
        if (played) return;
        played = true;

        gameObject.SetActive(true);

        if (animator != null)
        {
            animator.SetTrigger("PlayEffect");
        }
        else
        {
            Debug.LogWarning("No Animator assigned on EffectRunner!");
        }

        StartCoroutine(RunEffect());
    }

    IEnumerator RunEffect()
    {
        // Wait for animation to play out (timing buffer)
        yield return new WaitForSeconds(delayBeforeTrigger);

        // AFTER effect finishes → trigger UI
        if (isWin)
            fadeManager.ShowWin();
        else
            fadeManager.ShowFail();
    }
}