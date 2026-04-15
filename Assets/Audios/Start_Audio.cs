using UnityEngine;

public class Start_Audio : MonoBehaviour
{
    public AudioSource audioSource;

    // Call this from your button's OnClick()
    public void PlayAudio()
    {
        if (audioSource != null)
        {
            // Only play if it's NOT already playing
            // This prevents the sound from overlapping or restarting mid-way
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}