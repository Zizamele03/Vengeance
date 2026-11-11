using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video; // 👈 Needed for video handling

public class VictorySceneManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text titleText;
    public Text scoreText;
    public Text timeText;
    public Text victoryMessageText;

    [Header("Buttons")]
    public Button restartButton; // Goes to Main Menu
    public Button quitButton;    // Quits the game

    [Header("Video")]
    public VideoPlayer victoryVideo; // 👈 Assign your Video Player here in the Inspector

    [Header("Settings")]
    public string mainMenuScene = "MainMenu";

    void Start()
    {
        // Hide buttons at start
        if (restartButton != null) restartButton.gameObject.SetActive(false);
        if (quitButton != null) quitButton.gameObject.SetActive(false);

        // Setup button listeners
        if (restartButton != null)
            restartButton.onClick.AddListener(LoadMainMenu);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

        // Setup video end event
        if (victoryVideo != null)
        {
            victoryVideo.loopPointReached += OnVideoEnd; // 👈 triggers when video finishes
        }
        else
        {
            // No video? Just show victory screen immediately
            ShowVictory();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        ShowVictory(); // 👈 Display buttons and stats once video ends
    }

    void ShowVictory()
    {
        // Show buttons
        if (restartButton != null) restartButton.gameObject.SetActive(true);
        if (quitButton != null) quitButton.gameObject.SetActive(true);

        // Get values from PlayerPrefs
        int score = PlayerPrefs.GetInt("FinalScore", 0);
        float timeSeconds = PlayerPrefs.GetFloat("FinalTime", 0f);
        string timeFormatted = FormatTime(timeSeconds);

        // Update UI
        if (scoreText != null) scoreText.text = $"Score: {score}";
        if (timeText != null) timeText.text = $"Time: {timeFormatted}";
        if (titleText != null) titleText.text = "VICTORY!";
        if (victoryMessageText != null) victoryMessageText.text = "Congratulations! You've completed the level!";
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
