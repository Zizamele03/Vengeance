using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [SerializeField] private Health_Bar Healthbar;

    [Header("Components")]
    public Animator animator;
    public NavMeshAgent agent;
    public Transform target;

    private float maxHealth = 100;
    private float currentHealth = 100;

    [Header("Settings")]
    public float breathingDuration = 2f;   // Time to stay idle breathing
    public float combatWalkRange = 20f;    // Distance to enter combat walk
    public float fightRange = 5f;          // Distance to start fighting
    public float normalSpeed = 15f;
    public float combatSpeed = 9f;

    private bool startedWalking = false;

    void Start()
    {
        if (animator == null || agent == null || target == null)
        {
            Debug.LogError("Animator, NavMeshAgent, or Target not assigned!");
            return;
        }

        // Initialize animator states
        animator.SetBool("isWalking", false);
        animator.SetBool("isFighting", false);

        Invoke(nameof(StartWalking), breathingDuration);
    }

    void StartWalking()
    {
        startedWalking = true;
        animator.SetBool("isWalking", true); // Start walking
    }

    void Update()
    {
        if (!startedWalking) return;

        float distance = Vector3.Distance(transform.position, target.position);

        // Animation and movement logic
        if (distance > combatWalkRange)
        {
            // Normal walk
            animator.SetBool("isWalking", true);
            animator.SetBool("isFighting", false);
            agent.speed = normalSpeed;
            agent.isStopped = false;
        }
        else if (distance <= combatWalkRange && distance > fightRange)
        {
            // Combat walk (isWalking = false)
            animator.SetBool("isWalking", false);
            animator.SetBool("isFighting", false);
            agent.speed = combatSpeed;
            agent.isStopped = false;
        }
        else if (distance <= fightRange)
        {
            // Fighting
            animator.SetBool("isWalking", false);
            animator.SetBool("isFighting", true);
            agent.isStopped = true;
        }

        // Move toward target
        if (!agent.isStopped)
            agent.SetDestination(target.position);

        // Smooth rotation
        if (agent.velocity.magnitude > 0.1f)
        {
            Quaternion lookRot = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (Healthbar != null)
            Healthbar.updateHealthBar(maxHealth, currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        animator.SetTrigger("Die");
        agent.isStopped = true;
    }
}
