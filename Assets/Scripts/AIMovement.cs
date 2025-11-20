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

    // Optional: assign a LaunchProjectile component (weapon) on this object or a child.
    // If not assigned, the script will try to find one in children at Start().
    public LaunchProjectile projectileWeapon;
    
    private Transform player;
    private NavMeshAgent agent;
    private float nextAttackTime;

    private void Start()
    {
        // Find the player
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        
        // Get and setup NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = moveSpeed;
            agent.stoppingDistance = followDistance;
            agent.autoBraking = true;
        }

        // Auto-find a LaunchProjectile on this object or its children if none assigned
        if (projectileWeapon == null)
        {
            projectileWeapon = GetComponentInChildren<LaunchProjectile>();
        }

        // If attacking is enabled, enforce a 2 second attack interval and initialize next attack time
        if (canAttack)
        {
            attackCooldown = 2f; // fire every 2 seconds as requested
            nextAttackTime = Time.time + attackCooldown; // first shot after cooldown

            // Ensure we have a reference to the player if not already assigned
            if (player == null)
            {
                var p = GameObject.FindGameObjectWithTag("Player");
                if (p != null) player = p.transform;
            }
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

        // Always look at player. If we're about to attack, snap to face the player so projectiles go toward them.
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Keep upright
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // If it's time to attack, snap to face the player for an accurate shot, otherwise smooth rotate
            if (canAttack && Time.time >= nextAttackTime)
            {
                transform.rotation = lookRotation;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }

        // Handle attacking if enabled. Fires every attackCooldown seconds regardless of distance to player.
        if (canAttack && Time.time >= nextAttackTime)
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (Time.time < nextAttackTime)
            return;

        if (projectileWeapon != null)
        {
            // TryAttack() returns true if the attack started successfully
            bool started = projectileWeapon.TryAttack();
            if (started)
            {
                nextAttackTime = Time.time + attackCooldown;
                Debug.Log($"{gameObject.name} fired projectile at player.");
            }
        }
        else
        {
            // No projectile weapon — fallback to debug message
            Debug.LogWarning($"{gameObject.name} has canAttack=true but no LaunchProjectile assigned.");
            nextAttackTime = Time.time + attackCooldown; // still enforce cooldown to avoid spamming logs
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
