using UnityEngine;
using System.Collections;

public class Minigame_EffectRunner : MonoBehaviour
{
    public Minigame_fade_manager fadeManager;

    public Animator animator;
    public float delayBeforeTrigger = 1f;

    private bool played;

    public void PlayEffect(bool isWin)
    {
        if (played) return;
        played = true;

        gameObject.SetActive(true);

        if (animator != null)
        {
            animator.SetTrigger("PlayEffect");
        }

        StartCoroutine(RunEffect(isWin));
    }

    IEnumerator RunEffect(bool isWin)
    {
        yield return new WaitForSeconds(delayBeforeTrigger);

        if (fadeManager == null)
        {
            Debug.LogError("FadeManager missing!");
            yield break;
        }

        if (isWin)
            fadeManager.ShowWin();
        else
            fadeManager.ShowFail();
    }
}