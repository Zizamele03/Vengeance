using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;
    private NavMeshAgent navAgent;

    public bool isDead;

    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    /*public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            int randomValue = Random.Range(0, 3);

            if (randomValue == 0)
                animator.SetTrigger("DIE1");
            else if (randomValue == 1)
                animator.SetTrigger("DIE2");
            else
                animator.SetTrigger("DIE3");

            GetComponent<CapsuleCollider>().enabled = false;

            isDead = true;
        }
        else
        {
            animator.SetTrigger("DAMAGE");
        }
    }*/

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0 && !isDead)
        {
            isDead = true;

            // Stop all movement and animation conflicts
            navAgent.isStopped = true;

            // Reset other states so no animation can interrupt death
            animator.SetBool("isPatroling", false);
            animator.SetBool("isChasing", false);
            animator.SetBool("isAttacking", false);


            // Pick random death animation
            int randomValue = Random.Range(0, 3);
            if (randomValue == 0)
                animator.SetTrigger("DIE1");
            else if (randomValue == 1)
                animator.SetTrigger("DIE2");
            else
                animator.SetTrigger("DIE3");

            // Kill Shot sound
            SoundManager.Instance.guardChannel.PlayOneShot(SoundManager.Instance.killShot);

            // Disable collider so enemy can’t be hit again
            GetComponent<CapsuleCollider>().enabled = false;
        }
        else if (!isDead)
        {
            animator.SetTrigger("DAMAGE");
        }
    }


    // Helper to ensure only ONE animation boolean is true at a time
    public void SetAnimationState(bool patrol, bool chase, bool attack)
    {
        animator.SetBool("isPatroling", patrol);
        animator.SetBool("isChasing", chase);
        animator.SetBool("isAttacking", attack);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f); // Attack range

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f); // Detection (Chase start)

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 5.5f); // Stop chasing
    }
}





/*using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField] private int HP = 100;
    private Animator animator;

    private NavMeshAgent navAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }


    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            int randomValue = Random.Range(0, 3);  // 0, 1 or 2

            if (randomValue == 0)
            {
                animator.SetTrigger("DIE1");
            }
            else if (randomValue == 1)
            {
                animator.SetTrigger("DIE2");
            }
            else
            {
                animator.SetTrigger("DIE3");
            }
            GetComponent<CapsuleCollider>().enabled = false;

        }
        else
        {
            animator.SetTrigger("DAMAGE");
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f); // Attacking, Stop Attacking

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f); // Detection (Start Chasing)

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 5.5f); // Stop Chasing
    }


    *//*// Update is called once per frame
    void Update()
    {
        if (navAgent.velocity.magnitude > 0.1f) 
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }*//*

}
*/