using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public GuardSpawnController guardController; // Reference to guard spawner
    private bool sceneSwitched = false;          // Prevents multiple loads

    void Update()
    {
        if (sceneSwitched || guardController == null)
            return;

        // Clean up any null references in the list (destroyed guards)
        guardController.currentGuardAlive.RemoveAll(g => g == null);

        // If no guards remain alive in the list, trigger scene change
        if (guardController.currentGuardAlive.Count == 0)
        {
            sceneSwitched = true;
            Debug.Log("All guards eliminated! Switching to Scene2...");
            SceneManager.LoadScene("Scene2");
        }
    }
}
