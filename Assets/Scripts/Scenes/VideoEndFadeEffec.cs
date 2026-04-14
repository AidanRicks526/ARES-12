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
        fadeImage.color = new Color(0, 0, 0, 0);
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (triggered) return;

        triggered = true;
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        float timer = 0f;

        // blink/fade cycle
        while (timer < effectDuration)
        {
            yield return Fade(1f);
            yield return Fade(0f);

            timer += pulseSpeed * 2f;
        }

        // Makes sures it ends fully visible (important tweak you requested)
        fadeImage.color = new Color(0, 0, 0, 0);

        // small pause so it finishes
        yield return new WaitForSeconds(0.2f);

        // change scene
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
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, targetAlpha);
    }
}