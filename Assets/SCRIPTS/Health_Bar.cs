using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    [SerializeField] private float health; // health
    [SerializeField] private Image HealthBarSprite; // Image component
    private Camera cam; // Reference to main camera

    private void Start()
    {
        cam = Camera.main;
    }

    public void takeDamage(float damage) // take damage from attack
    {
        health -= damage;
        Debug.Log(health);
    }
    public void updateHealthBar(float maxHealth, float currentHealth)
    {
        // Set fill amount between 0 and 1
        HealthBarSprite.fillAmount = currentHealth / maxHealth;
    }

    private void LateUpdate()
    {
        // Make health bar face the camera (only on Y axis so it doesn't tilt weirdly)
        Vector3 lookDir = transform.position - cam.transform.position;
        lookDir.y = 0; // lock Y so it doesn't roll/tilt
        transform.rotation = Quaternion.LookRotation(lookDir);
    }
}