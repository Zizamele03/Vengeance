using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroWithVengeanceAudio : MonoBehaviour
{
    [Header("Video & Audio")]
    public VideoPlayer videoPlayer;
    public AudioSource introAudio;       // Intro song
    public AudioSource vengeanceAudio;   // Vengeance song

    [Header("Scene")]
    public string mainMenuSceneName = "MainMenu";
    public float videoLength = 17f;      // Length of the video in seconds

    void Start()
    {
        // Play video and intro audio together
        videoPlayer.Play();
        introAudio.Play();

        // Start coroutine to wait for video then play vengeance audio
        StartCoroutine(PlayVengeanceAfterVideo());
    }

    IEnumerator PlayVengeanceAfterVideo()
    {
        // Wait for video duration
        yield return new WaitForSeconds(videoLength);

        // Stop intro audio if still playing
        if (introAudio.isPlaying)
            introAudio.Stop();

        // Play vengeance audio
        vengeanceAudio.Play();

        // Optional: wait for vengeance audio to finish, then load main menu
        yield return new WaitForSeconds(vengeanceAudio.clip.length);

        // Load main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
