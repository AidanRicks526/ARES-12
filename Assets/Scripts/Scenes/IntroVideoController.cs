using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject videoObject;   // RawImage or video panel
    public GameObject playButton;

    void Start()
    {
        // Show video frame but keep it paused
        videoObject.SetActive(true);

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnPrepared;
        videoPlayer.loopPointReached += OnVideoEnd;

        playButton.SetActive(true);
    }

    void OnPrepared(VideoPlayer vp)
    {
        vp.Pause(); // freeze on first frame (background look)
    }

    public void PlayVideo()
    {
        playButton.SetActive(false);

        videoPlayer.time = 0;
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        videoObject.SetActive(false); // instantly hide video
    }
}