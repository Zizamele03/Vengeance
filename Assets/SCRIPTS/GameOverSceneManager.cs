using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameOverSceneManager : MonoBehaviour
{
    [Header("Video")]
    public VideoPlayer gameOverVideo; // The video to play before showing UI

    [Header("UI Elements")]
    public Text titleText;
    public Text scoreText;
    public Text timeText;
    public Text causeOfDeathText;

    [Header("Buttons")]
    public Button restartButton; // Goes to Main Menu
    public Button quitButton;    // Quits the game

    [Header("Settings")]
    public string mainMenuScene = "MainMenu";

    void Start()
    {
        // Hide buttons at the start (until the video ends)
        if (restartButton != null) restartButton.gameObject.SetActive(false);
        if (quitButton != null) quitButton.gameObject.SetActive(false);

        // Setup button listeners
        if (restartButton != null)
            restartButton.onClick.AddListener(LoadMainMenu);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

        // Handle video playback and event
        if (gameOverVideo != null)
        {
            gameOverVideo.loopPointReached += OnVideoEnd;
            gameOverVideo.Play();
        }
        else
        {
            // No video assigned — show UI immediately
            ShowGameOver();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("🎬 Game Over video finished!");
        ShowGameOver();
    }

    void ShowGameOver()
    {
        // Show UI elements
        if (restartButton != null) restartButton.gameObject.SetActive(true);
        if (quitButton != null) quitButton.gameObject.SetActive(true);

        // Retrieve stats from PlayerPrefs
        int score = PlayerPrefs.GetInt("FinalScore", 0);
        float timeSeconds = PlayerPrefs.GetFloat("FinalTime", 0f);
        string causeOfDeath = PlayerPrefs.GetString("CauseOfDeath", "You died");
        string timeFormatted = FormatTime(timeSeconds);

        // Update UI text
        if (scoreText != null) scoreText.text = $"Score: {score}";
        if (timeText != null) timeText.text = $"Time: {timeFormatted}";
        if (titleText != null) titleText.text = "GAME OVER";
        if (causeOfDeathText != null) causeOfDeathText.text = causeOfDeath;
    }

    string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        return $"{minutes:00}:{secs:00}";
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
