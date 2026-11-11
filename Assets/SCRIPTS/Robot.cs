using UnityEngine;
using UnityEngine.AI;

public class RobotNavigator : MonoBehaviour
{
    [Header("Target to Move Toward")]
    public Transform target; // The sphere target

    private NavMeshAgent agent;

    void Start()
    {
        // Get the NavMeshAgent component on this GameObject
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("No NavMeshAgent found on this GameObject!");
        }

        if (target == null)
        {
            Debug.LogWarning("No target assigned! Please drag your target (sphere) into the inspector.");
        }
    }

    void Update()
    {
        // Continuously update the destination if the target exists
        if (target != null && agent != null)
        {
            agent.SetDestination(target.position);
        }
    }
}
