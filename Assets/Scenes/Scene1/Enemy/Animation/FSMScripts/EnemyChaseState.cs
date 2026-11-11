using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    public float chaseSpeed = 4f;
    public float stopChaseDistance = 6f;
    public float attackDistance = 1.8f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;

        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = chaseSpeed;

        // Chase only
        animator.GetComponent<Enemy>().SetAnimationState(false, true, false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null) return;

        agent.SetDestination(player.position);
        animator.transform.LookAt(player);

        float distance = Vector3.Distance(animator.transform.position, player.position);

        // Player got close → attack
        if (distance <= attackDistance)
        {
            animator.GetComponent<Enemy>().SetAnimationState(false, false, true);
        }
        // Player too far → stop chasing
        else if (distance > stopChaseDistance)
        {
            animator.GetComponent<Enemy>().SetAnimationState(true, false, false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }
}




/*using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;

    public float chaseSpeed = 6f;

    public float stopChasingDistance = 21f;
    public float attackingDistance = 2.5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = chaseSpeed;
    }

    // OnStateUpdate is called on each update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        animator.transform.LookAt(player);

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        if (distanceFromPlayer > stopChasingDistance)
            animator.SetBool("isChasing", false);

        if (distanceFromPlayer < attackingDistance)
            animator.SetBool("isAttacking", true);
            animator.SetBool("isChasing", false); // Stop chasing before attacking
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }
}
*/