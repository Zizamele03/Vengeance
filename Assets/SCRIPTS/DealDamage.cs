using UnityEngine;
using System.Collections.Generic;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private float damage = 10f; // Damage dealt per hit
    private Collider col; // Collider reference (e.g., hand or foot)

    [Header("Blood Effect")]
    [SerializeField] private GameObject bloodPrefab; // Particle system prefab
    [SerializeField] private float bloodLifetime = 1f; // Lifetime before destruction

    // Track enemies hit during one punch (prevents double-hits in one animation)
    private HashSet<HealthSystem> hitEnemies = new HashSet<HealthSystem>();

    private void Awake()
    {
        // Get and configure collider
        col = GetComponent<Collider>();
        col.isTrigger = true;
        col.enabled = false; // disabled by default (enabled via animation event)
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!col.enabled) return; // Ignore if not during an active punch

        // Try to get a HealthSystem on the object we hit
        HealthSystem enemy = other.GetComponent<HealthSystem>();

        // Skip if no HealthSystem or already hit this frame
        if (enemy == null || hitEnemies.Contains(enemy))
            return;

        // ? Determine the exact hit point for directional reactions
        Vector3 hitPoint = other.ClosestPoint(transform.position);

        // Log for clarity
        Debug.Log($"Dealing {damage} damage to: {other.gameObject.name} at {hitPoint}");

        // Apply damage *with hit position*
        enemy.takeDamage(damage, transform.position);

        // Mark this enemy as already hit during this swing
        hitEnemies.Add(enemy);

        // ------------------ BLOOD SPAWN ------------------
        if (bloodPrefab != null)
        {
            // Raycast from fist to enemy to get surface normal for blood effect
            Vector3 direction = (other.transform.position - transform.position).normalized;
            if (other.Raycast(new Ray(transform.position, direction), out RaycastHit hit, 1.5f))
            {
                SpawnBlood(hit.point, hit.normal);
            }
            else
            {
                // Fallback spawn position if raycast fails
                Vector3 spawnPos = other.ClosestPoint(transform.position);
                SpawnBlood(spawnPos, -other.transform.forward);
            }
        }
    }

    // Spawns and plays the blood effect
    private void SpawnBlood(Vector3 position, Vector3 normal)
    {
        GameObject blood = Instantiate(
            bloodPrefab,
            position + normal * 0.02f, // small offset to avoid clipping
            Quaternion.LookRotation(normal)
        );

        ParticleSystem ps = blood.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
        }

        Destroy(blood, bloodLifetime);
    }

    // Called by animation event when attack starts
    public void EnableDamage()
    {
        col.enabled = true;
        hitEnemies.Clear(); // reset for new attack window
        Debug.Log($"{gameObject.name} collider ENABLED for attack");
    }

    // Called by animation event when attack ends
    public void DisableDamage()
    {
        col.enabled = false;
        Debug.Log($"{gameObject.name} collider DISABLED after attack");
    }
}