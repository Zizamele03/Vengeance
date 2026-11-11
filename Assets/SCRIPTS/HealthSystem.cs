using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] public float maxHealth = 100f;
    public float currentHealth;
    public bool alarmTrigger = false;

    [Header("UI Reference")]
    [SerializeField] private Health_Bar healthBar;

    [Header("Hit Reaction Settings")]
    [SerializeField] private float hitReactionCooldown = 0.5f;
    private bool canReact = true;

    private Animator animator;

    // --- Boss reaction tracking ---
    private float nextBossReactionThreshold;
    private bool bossStartedReacting = false;
    private HashSet<float> sweepTriggers = new HashSet<float>(); // tracks 20% and 10% sweeps done

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogWarning($"{gameObject.name} has no Animator attached.");
    }

    private void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
            healthBar.updateHealthBar(maxHealth, currentHealth);

        // Boss won't react until below 50%
        nextBossReactionThreshold = 50f;
    }

    public void takeDamage(float damage, Vector3 attackerPosition)
    {
        float oldHealth = currentHealth;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        // Update UI
        if (healthBar != null && healthBar.gameObject.activeInHierarchy)
        {
            healthBar.updateHealthBar(maxHealth, currentHealth);
            if (AudioManager.Instance != null) AudioManager.Instance.Play("Punch");
        }

        // Calculate direction of hit
        Vector3 toAttacker = (attackerPosition - transform.position);
        toAttacker.y = 0f;
        if (toAttacker.sqrMagnitude < 0.0001f) toAttacker = transform.forward;
        toAttacker.Normalize();
        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();
        float angle = Vector3.SignedAngle(forward, toAttacker, Vector3.up);
        string directionLabel = Mathf.Abs(angle) > 135f ? "Back" : "Front";

        Debug.Log($"[{gameObject.name}] Hit from {directionLabel} | Health: {currentHealth}");

        if (gameObject.name.Contains("Marco"))
        {
            HandleBossReactions(directionLabel, oldHealth);
        }
        else
        {
            if (currentHealth > 0f && animator != null && canReact)
                StartCoroutine(PlayDirectionalHitReaction(directionLabel));
        }

        if (currentHealth <= 50f) alarmTrigger = true;
        if (currentHealth <= 0f)
        {
            alarmTrigger = false;
            Die();
        }
    }

    public void takeDamage(float damage)
    {
        takeDamage(damage, transform.position + transform.forward * 0.5f);
    }

    // ================== BOSS REACTIONS ==================
    private void HandleBossReactions(string direction, float oldHealth)
    {
        float healthPercent = (currentHealth / maxHealth) * 100f;
        float oldPercent = (oldHealth / maxHealth) * 100f;

        // Only start reacting once below 50%
        if (!bossStartedReacting)
        {
            if (healthPercent <= 50f)
            {
                bossStartedReacting = true;
                // set next threshold to floor to the nearest 10 below current percent (e.g. if healthPercent=47 -> 40)
                nextBossReactionThreshold = Mathf.Floor(healthPercent / 10f) * 10f;
                if (nextBossReactionThreshold <= 0f) nextBossReactionThreshold = 0f;
                Debug.Log($"[Marco] Started reacting below 50%. Next threshold: {nextBossReactionThreshold}%");
                PlayBossHitReaction(direction);
            }
            return;
        }

        // Prevent overlapping reactions
        if (!canReact) return;

        // Trigger normal or special reactions based on thresholds
        if (healthPercent <= nextBossReactionThreshold)
        {
            // --- Special Sweep Reactions at 20% and 10% ---
            if ((Mathf.Approximately(nextBossReactionThreshold, 20f) || Mathf.Approximately(nextBossReactionThreshold, 10f))
                && !sweepTriggers.Contains(nextBossReactionThreshold))
            {
                sweepTriggers.Add(nextBossReactionThreshold);
                Debug.Log($"[Marco] Special sweep + kickup triggered at {nextBossReactionThreshold}% health!");
                StartCoroutine(PlayBossSweepSequence());
            }
            else
            {
                PlayBossHitReaction(direction);
            }

            // Move to next threshold (every 10%)
            nextBossReactionThreshold -= 10f;
            nextBossReactionThreshold = Mathf.Max(nextBossReactionThreshold, 0f);
        }
    }

    private void PlayBossHitReaction(string direction)
    {
        if (animator == null) return;

        StartCoroutine(BossReactionCooldown());

        string trigger = $"Hit_{direction}";
        if (HasAnimatorParameter(animator, trigger, AnimatorControllerParameterType.Trigger))
        {
            ResetHitTriggersSafe();
            animator.SetTrigger(trigger);
            Debug.Log($"[Marco] Triggered {trigger}");
        }
        else
        {
            Debug.LogWarning($"[Marco] Animator missing trigger: {trigger}");
        }
    }

    private IEnumerator PlayBossSweepSequence()
    {
        if (animator == null) yield break;
        canReact = false;

        // Play sweep fall
        if (HasAnimatorParameter(animator, "Hit_SweepFall", AnimatorControllerParameterType.Trigger))
        {
            ResetHitTriggersSafe();
            animator.SetTrigger("Hit_SweepFall");
            Debug.Log("[Marco] Playing Hit_SweepFall");
        }
        else
        {
            Debug.LogWarning("[Marco] Animator missing 'Hit_SweepFall' trigger");
        }

        // Wait for fall to finish (adjust timing to match clip length)
        yield return new WaitForSeconds(2.0f);

        // Then kick up
        if (HasAnimatorParameter(animator, "KickUp", AnimatorControllerParameterType.Trigger))
        {
            ResetHitTriggersSafe();
            animator.SetTrigger("KickUp");
            Debug.Log("[Marco] Playing KickUp");
        }
        else
        {
            Debug.LogWarning("[Marco] Animator missing 'KickUp' trigger");
        }

        // Give a short cooldown
        yield return new WaitForSeconds(1.5f);
        canReact = true;
    }

    private IEnumerator PlayDirectionalHitReaction(string directionLabel)
    {
        canReact = false;

        string triggerName = $"Hit_{directionLabel}";
        if (HasAnimatorParameter(animator, triggerName, AnimatorControllerParameterType.Trigger))
        {
            ResetHitTriggersSafe();
            animator.SetTrigger(triggerName);
            Debug.Log($"[{gameObject.name}] Triggered {triggerName}");
        }

        yield return new WaitForSeconds(hitReactionCooldown);
        canReact = true;
    }

    private IEnumerator BossReactionCooldown()
    {
        canReact = false;
        yield return new WaitForSeconds(hitReactionCooldown);
        canReact = true;
    }

    private void ResetHitTriggersSafe()
    {
        string[] possible = { "Hit_Front", "Hit_Back", "Hit_SweepFall", "KickUp" };
        foreach (var p in possible)
        {
            if (HasAnimatorParameter(animator, p, AnimatorControllerParameterType.Trigger))
                animator.ResetTrigger(p);
        }
    }

    private bool HasAnimatorParameter(Animator anim, string paramName, AnimatorControllerParameterType type)
    {
        if (anim == null) return false;
        foreach (var p in anim.parameters)
            if (p.type == type && p.name == paramName)
                return true;
        return false;
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");

        if (animator != null && HasAnimatorParameter(animator, "Die", AnimatorControllerParameterType.Trigger))
            animator.SetTrigger("Die");

        if (gameObject.name == "John")
            SceneManager.LoadScene("GameOverScene");
        else if (gameObject.name.Contains("Marco"))
            SceneManager.LoadScene("VictoryScene");
    }
}