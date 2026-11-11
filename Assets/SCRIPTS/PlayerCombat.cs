using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Damage Colliders (hands, feet, weapons etc.)")]
    [SerializeField] private DealDamage[] damageColliders; // assign in inspector

    // Enable a specific collider (called by Animation Event)
    public void EnableCollider(int index)
    {
        if (index >= 0 && index < damageColliders.Length)
        {
            damageColliders[index].EnableDamage();
        }
    }

    // Disable a specific collider (called by Animation Event)
    public void DisableCollider(int index)
    {
        if (index >= 0 && index < damageColliders.Length)
        {
            damageColliders[index].DisableDamage();
        }
    }
}
