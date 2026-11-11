using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    public float stopAttackDistance = 2.2f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;

        agent = animator.GetComponent<NavMeshAgent>();

        // Attack only
        animator.GetComponent<Enemy>().SetAnimationState(false, false, true);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null) return;

        // Face player
        Vector3 direction = player.position - agent.transform.position;
        direction.y = 0;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        float distance = Vector3.Distance(animator.transform.position, player.position);

        // If player moves away → chase again
        if (distance > stopAttackDistance)
        {
            animator.GetComponent<Enemy>().SetAnimationState(false, true, false);
        }
    }
}





/*using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;

    public float stopAttackingDistance = 2.5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LookAtPlayer();

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        if (distanceFromPlayer > stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    // Make the Enemy always look at the player
    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        direction.y = 0; // Prevent tilting up/down
        agent.transform.rotation = Quaternion.LookRotation(direction);
    }
}
*/