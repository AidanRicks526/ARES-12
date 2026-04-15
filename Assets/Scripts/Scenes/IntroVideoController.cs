using UnityEngine;
using UnityEngine.Video;

public class IntroVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject videoObject;
    public GameObject playButton;
    public GameObject Logo;

    public GameObject skipButton;

    public VideoEndSceneTransition transition; // reference

    void Start()
    {
        videoObject.SetActive(true);

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnPrepared;

        playButton.SetActive(true);
        Logo.SetActive(true);

        skipButton.SetActive(false);
    }

    void OnPrepared(VideoPlayer vp)
    {
        vp.Pause();

        // Fetch the volume the user saved in the settings menu
        float savedVolume = PlayerPrefs.GetFloat("volume", 1f);

        // Apply it to the video player
        vp.SetDirectAudioVolume(0, savedVolume);
    }

    public void PlayVideo()
    {
        playButton.SetActive(false);
        Logo.SetActive(false);

        skipButton.SetActive(true);

        videoPlayer.time = 0;
        videoPlayer.Play();
    }

    public void SkipVideo()
    {
        videoPlayer.Stop();

        videoObject.SetActive(false);
        skipButton.SetActive(false);

        transition.TriggerTransition();
    }
}