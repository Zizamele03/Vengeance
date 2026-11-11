using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ⬅️ Needed for scene switching

public class GuardSpawnController : MonoBehaviour
{
    public int initialGuardPerWave = 3;
    public int currentGuardPerWave;

    public float spawnDelay = 0.5f; // Delay between spawning each Guard in wave

    public int currentWave = 0;
    public float waveCooldown = 10.0f;

    public bool inCooldown;
    public float cooldownCounter = 0f;

    public List<Enemy> currentGuardAlive;

    public GameObject guardPrefab;

    private bool sceneSwitched = false; // To avoid multiple loads

    private void Start()
    {
        if (currentGuardAlive == null)
            currentGuardAlive = new List<Enemy>();

        currentGuardPerWave = initialGuardPerWave;
        cooldownCounter = waveCooldown;

        StartNextWave();
    }

    private void StartNextWave()
    {
        currentGuardAlive.Clear();
        currentWave++;
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        if (guardPrefab == null)
        {
            Debug.LogError("Guard prefab is not assigned!");
            yield break;
        }

        for (int i = 0; i < currentGuardPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            var guard = Instantiate(guardPrefab, spawnPosition, Quaternion.identity);
            Enemy enemyScript = guard.GetComponent<Enemy>();
            if (enemyScript != null)
                currentGuardAlive.Add(enemyScript);
            else
                Debug.LogWarning("Instantiated guard has no Enemy component.");

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        if (currentGuardAlive == null)
            currentGuardAlive = new List<Enemy>();

        currentGuardAlive.RemoveAll(g => g == null || g.isDead);

        // ✅ When all guards are dead, switch to Scene2
        if (currentGuardAlive.Count == 0 && !sceneSwitched)
        {
            sceneSwitched = true;
            Debug.Log("✅ All guards eliminated. Loading Scene2...");
            SceneManager.LoadScene("Scene2");
        }

        // Handle cooldown timer (optional)
        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
            if (cooldownCounter < 0f) cooldownCounter = 0f;
        }
        else
        {
            cooldownCounter = waveCooldown;
        }
    }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;
        cooldownCounter = waveCooldown;

        yield return new WaitForSeconds(waveCooldown);

        inCooldown = false;
        currentGuardPerWave *= 2;
        StartNextWave();
    }
}




/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSpawnController : MonoBehaviour
{
    public int initialGuardPerWave = 3;
    public int currentGuardPerWave;

    public float spawnDelay = 0.5f; // Delay between spawning each Guard in wave

    public int currentWave = 0;
    public float waveCooldown = 10.0f;

    public bool inCooldown;
    public float cooldownCounter = 0f;

    public List<Enemy> currentGuardAlive;

    public GameObject guardPrefab;

    private void Start()
    {
        // initialize
        if (currentGuardAlive == null)
            currentGuardAlive = new List<Enemy>();

        currentGuardPerWave = initialGuardPerWave;
        cooldownCounter = waveCooldown;

        StartNextWave();
    }

    private void StartNextWave()
    {
        // clear list safely
        currentGuardAlive.Clear();

        currentWave++;
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        if (guardPrefab == null)
        {
            Debug.LogError("Guard prefab is not assigned!");
            yield break;
        }

        // spawn 'currentGuardPerWave' guards with a small delay between each
        for (int i = 0; i < currentGuardPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            var guard = Instantiate(guardPrefab, spawnPosition, Quaternion.identity);
            Enemy enemyScript = guard.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                currentGuardAlive.Add(enemyScript);
            }
            else
            {
                Debug.LogWarning("Instantiated guard has no Enemy component.");
            }

            // wait between spawns
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        if (currentGuardAlive == null)
            currentGuardAlive = new List<Enemy>();

        List<Enemy> guardsToRemove = new List<Enemy>();
        foreach (Enemy guard in currentGuardAlive)
        {
            if (guard == null || guard.isDead)
            {
                guardsToRemove.Add(guard);
            }
        }

        // remove dead/null guards
        foreach (Enemy guard in guardsToRemove)
        {
            currentGuardAlive.Remove(guard);
        }

        // Start cooldown if all guards are dead and not already in cooldown
        if (currentGuardAlive.Count == 0 && !inCooldown)
        {
            StartCoroutine(WaveCooldown());
        }

        // Run the cooldown counter (for UI or debugging)
        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
            if (cooldownCounter < 0f) cooldownCounter = 0f;
        }
        else
        {
            cooldownCounter = waveCooldown;
        }
    }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;
        cooldownCounter = waveCooldown;

        yield return new WaitForSeconds(waveCooldown);

        inCooldown = false;

        // increase guards per wave for next wave
        currentGuardPerWave *= 2; // e.g., 5 -> 10
        StartNextWave();
    }
}





using UnityEngine;

public class GuardSpawnController : MonoBehaviour
{
    public int initialGuardPerWave = 5;
    public int currentGuardPerWave;

    public float spawnDelay = 0.5f; // Delay between spawning each Guard in wave

    public int currentWave = 0;
    public float waveCooldown = 10.0f;

    public bool inCooldown;
    public float cooldownCounter = 0;

    public List<Enemy> currentGuardAlive;

    public GameObject guardPrefab;

    private void Start()
    {
        initialGuardPerWave = initialGuardPerWave;

        StartNextWave();
    }

    private voidStartNextWave()
    {
        currentGuardAlive.Clear();

        currentWave++;

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        // Generate a random offset wuthin a specified range
        Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        Vector3 spawnPosition = transform.position + spawnOffset;

        // Instantiate the Guard
        var zombie Instantiate(guardPrefab, spawnPosition, Quaternion.identity);

        // Get Enemy Script
        Enemy enemyScript = guard.GetComponent<Enemy>();

        // Track this Guard
        currentGuardAlive.Add(enemyScript);

        yield return WaitForSeconds(spawnDelay);
    }

    private void Update()
    {
        List<Enemy> guardsToRemove = new List<Enemy>();
        foreach (Enemy guard in currentGuardAlive)
        {
            if (guard.isDead)
            {
                guardsToRemove.Add(guard);
            }
        }

        // Actually remove all the guards
        foreach (Enemy guard in currentGuardAlive)
        {
            currentGuardAlive.Remove(guard);
        }

        guardsToRemove.Clear();

        // Start cooldown if all guards are dead
        if (currentGuardAlive.Count == 0 && inCooldown == false)
        {
            // Start the next cool down for the next wave
            StartCoroutine(WaveCooldown());
        }

        // Run the cooldown counter
        if (inCoolDown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            // Reset the counter
            cooldownCounter = waveCooldown;
        }
    }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;

        yield return new WaitforSeconds(waveCooldown);

        inCooldown = false;

        currentGuardPerWave *= 2; // 5*2 = 10
        StartNextWave();
    }
}*/