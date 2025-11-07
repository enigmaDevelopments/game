using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float followDistance = 2f;     // How close the AI gets to player
    
    [Header("Optional Attack Settings")]
    public bool canAttack = false;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    
    private Transform player;
    private NavMeshAgent agent;
    private float nextAttackTime;

    private void Start()
    {
        // Find the player
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Get and setup NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = moveSpeed;
            agent.stoppingDistance = followDistance;
            agent.autoBraking = true;
        }
    }

    private void Update()
    {
        if (player == null || agent == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Always try to follow player
        if (agent.isOnNavMesh)
        {
            // Update destination every frame to follow player
            agent.SetDestination(player.position);

            // Handle attacking if enabled
            if (canAttack && distanceToPlayer <= attackRange)
            {
                TryAttack();
            }

            // Debug info
            if (!agent.hasPath)
            {
                Debug.Log("No path to player");
            }
            else if (agent.pathStatus == NavMeshPathStatus.PathPartial)
            {
                Debug.Log("Partial path to player");
            }
        }
        else
        {
            Debug.LogWarning("Agent not on NavMesh!");
        }

        // Always look at player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Keep upright
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void TryAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            // Implement your attack logic here
            Debug.Log($"{gameObject.name} is attacking!");
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    // Optional: Visualize attack range in editor
    private void OnDrawGizmosSelected()
    {
        if (canAttack)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
