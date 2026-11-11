using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomSceneButton : MonoBehaviour
{
    void OnMouseDown()
    {
        int sceneIndex = Random.Range (0, 2);//0 or 1

        if(sceneIndex == 0)
        {
            SceneManager.LoadScene("VictoryScene");
        }
        else
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
