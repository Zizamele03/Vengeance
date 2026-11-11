using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    [Header("Red Alarm Light")]
    public Light targetLight;
    public float onIntensity = 2f;
    public float offIntensity = 0.8f;
    public float onDuration = 0.50f;
    public float offDuration = 0.50f;

    [Header("Main Light (Scene)")]
    public Light mainLight;
    public float dimmedIntensity = 0f;
    private float originalMainIntensity;

    [Header("Health System Reference")]
    public HealthSystem healthSystem; // Drag the GameObject with HealthSystem here

    private bool isLockdown = false;
    private bool isOn = false;
    private float timer = 0f;

    void Start()
    {
        if (mainLight != null)
            originalMainIntensity = mainLight.intensity;

        if (targetLight != null)
        {
            targetLight.enabled = false;
            targetLight.intensity = offIntensity;
        }

        // Try to find HealthSystem if not assigned
        if (healthSystem == null)
        {
            healthSystem = FindObjectOfType<HealthSystem>();
        }
    }

    void Update()
    {
        // Check if we have a reference to HealthSystem
        if (healthSystem != null)
        {
            // Activate lockdown based on current health
            if (healthSystem.currentHealth <= 50 && healthSystem.currentHealth>0)
            {
                ActivateLockdown();
            }
            else
            {
                DeactivateLockdown();
            }

            // Make flicker speed change based on health (optional)
            float healthPercent = healthSystem.currentHealth / healthSystem.maxHealth;
            float flickerSpeed = Mathf.Lerp(0.2f, 1f, healthPercent); // Faster flicker when health is low

            // Handle flicker
            if (isLockdown && targetLight != null)
            {
                timer -= Time.deltaTime * flickerSpeed;

                if (timer <= 0f)
                {
                    isOn = !isOn;
                    targetLight.intensity = isOn ? onIntensity : offIntensity;
                    timer = isOn ? onDuration : offDuration;
                }
            }
        }

        // Optional: Keep manual L key toggle for testing
        if (Input.GetKeyDown(KeyCode.L))
        {
            ToggleManualLockdown();
        }
    }

    private void ActivateLockdown()
    {
        if (!isLockdown)
        {
            isLockdown = true;

            if (mainLight != null)
                mainLight.intensity = dimmedIntensity;

            if (targetLight != null)
                targetLight.enabled = true;

            // Play siren
            if (AudioManager.Instance != null)
                AudioManager.Instance.Play("LockdownSiren");
        }
    }

    private void DeactivateLockdown()
    {
        if (isLockdown)
        {
            isLockdown = false;

            if (mainLight != null)
                mainLight.intensity = originalMainIntensity;

            if (targetLight != null)
                targetLight.enabled = false;

            // Stop siren
            if (AudioManager.Instance != null)
                AudioManager.Instance.Stop("LockdownSiren");
        }
    }

    private void ToggleManualLockdown()
    {
        isLockdown = !isLockdown;

        if (mainLight != null)
            mainLight.intensity = isLockdown ? dimmedIntensity : originalMainIntensity;

        if (targetLight != null)
            targetLight.enabled = isLockdown;

        // Play/stop siren
        if (isLockdown)
            AudioManager.Instance.Play("LockdownSiren");
        else
            AudioManager.Instance.Stop("LockdownSiren");
    }
}