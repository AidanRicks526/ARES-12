using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class VideoEndSceneTransition : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Image fadeImage;

    public float effectDuration = 3f;
    public float pulseSpeed = 0.25f;

    public string sceneToLoad;

    private bool triggered = false;

    void Start()
    {
        // start fully transparent black
        SetBlack(0f);

        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        TriggerTransition();
    }

    public void TriggerTransition()
    {
        if (triggered) return;

        triggered = true;
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        float timer = 0f;

        // blinking phase
        while (timer < effectDuration)
        {
            yield return Fade(1f); // black visible
            yield return Fade(0f); // transparent

            timer += pulseSpeed * 2f;
        }

        // force FULL BLACK (no transparency)
        SetBlack(1f);

        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float t = 0f;

        while (t < pulseSpeed)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(startAlpha, targetAlpha, t / pulseSpeed);
            SetBlack(a);
            yield return null;
        }

        SetBlack(targetAlpha);
    }

    void SetBlack(float alpha)
    {
        fadeImage.color = new Color(0f, 0f, 0f, alpha);
    }
}