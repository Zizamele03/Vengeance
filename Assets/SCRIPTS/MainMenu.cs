using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSimple : MonoBehaviour
{
    public void PlayIntro()
    {
        SceneManager.LoadScene("IntroScene");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Scene1");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}