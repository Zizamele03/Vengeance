using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Scene Names")]
    public string victoryScene = "VictoryScene";
    public string gameOverScene = "GameOverScene";
    public string mainMenuScene = "MainMenu";
    public string gameScene = "GameScene";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerWins(int score, float time)
    {
        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.SetFloat("FinalTime", time);
        SceneManager.LoadScene(victoryScene);
    }

    public void PlayerLoses(int score, float time, string causeOfDeath = "You died")
    {
        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.SetFloat("FinalTime", time);
        PlayerPrefs.SetString("CauseOfDeath", causeOfDeath);
        SceneManager.LoadScene(gameOverScene);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}