using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    #if UNITY_EDITOR
        public bool sight;
        public bool runAway;
        public bool detection;
        public bool hasWeapon;
        public bool omniscient;
    #endif

    public LayerMask enviromentMask;
    public float runAwayRadius;
    public float detectionRadius;
    public bool raycast;
    public float veiwAngle;
    public float veiwRadius;
    public float turningSpeed;
    public AttackBase attack;
    public float attackAngle;
    public static bool playerInvisable = false;

    private NavMeshAgent agent;
    private Transform player;
    private Rigidbody rb;
    private Vector3 lastSelfDirection;
    private Vector3 lastDirection;
    private float turningTime;

    private bool AgentReady()
    {
        return agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh;
    }
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        // Initialize directions to avoid zero-vector LookRotation
        lastSelfDirection = transform.forward;
        lastDirection = transform.forward;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerInvisable)
            return;
        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 direction = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, direction);
        bool agentReady = AgentReady();
        #region Detection
        if (distance < detectionRadius || 
        (distance < veiwRadius &&
         angle  < veiwAngle / 2 &&
        (!raycast || !Physics.Raycast(transform.position, direction, distance, enviromentMask))))
        {
            #region On Player Detection
            lastDirection = direction;
            if (agentReady)
            {
                agent.SetDestination(player.position + runAwayRadius * -direction);
            }
            // attack logic
            if (angle < attackAngle/2 && agentReady && agent.remainingDistance <= agent.stoppingDistance)
                attack.TryAttack();
            #endregion
        }
        #endregion

        #region Turning
        if (agentReady && agent.remainingDistance <= agent.stoppingDistance && turningTime < 1)
        {
            turningTime += turningSpeed;
            rb.MoveRotation(Quaternion.Slerp(Quaternion.LookRotation(lastSelfDirection), Quaternion.LookRotation(lastDirection), turningTime));
        }
        else
        {
            turningTime = 0;
            lastSelfDirection = transform.forward;
        }
        #endregion

        #if UNITY_EDITOR
            Debug.DrawRay(transform.position, direction * distance, Color.red);
            if (!agentReady)
            {
                Debug.DrawRay(transform.position, Vector3.up, Color.yellow);
            }
        #endif
    }
}