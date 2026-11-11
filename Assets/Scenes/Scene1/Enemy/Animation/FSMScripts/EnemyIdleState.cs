using UnityEngine;

public class EnemyIdleState : StateMachineBehaviour
{
    private float timer;
    public float idleTime = 3f;
    private Transform player;
    public float detectionRadius = 5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;

        // When idle starts, only patrol is false and idle waiting
        animator.GetComponent<Enemy>().SetAnimationState(false, false, false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        // If player detected nearby → chase
        if (player != null && Vector3.Distance(animator.transform.position, player.position) <= detectionRadius)
        {
            animator.GetComponent<Enemy>().SetAnimationState(false, true, false);
            return;
        }

        // After some idle time → patrol
        if (timer > idleTime)
        {
            animator.GetComponent<Enemy>().SetAnimationState(true, false, false);
        }
    }
}






/*using UnityEngine;

public class EnemyIdleState : StateMachineBehaviour
{
    float timer;
    public float idleTime = 0f; // give it a small default value
    Transform player;
    public float detectionAreaRadius = 5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Transition to patrol state
        timer += Time.deltaTime;
        if (timer > idleTime)
        {
            animator.SetBool("isPatroling", true);
        }

        // Transition to Chase state
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionAreaRadius)
        {
            animator.SetBool("isChasing", true);
        }
    }
}
*/