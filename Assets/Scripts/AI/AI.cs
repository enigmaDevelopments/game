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
        public bool lookAtPlayer;
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
    public Transform rotaionTransform;
    public Transform head;

    private NavMeshAgent agent;
    private Transform player;
    private Vector3 lastDirection;

    private bool AgentReady()
    {
        return agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh;
    }
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        // Initialize directions to avoid zero-vector LookRotation
        lastDirection = head.forward;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerInvisable)
            return;
        float distance = Vector3.Distance(head.position, player.position);
        Vector3 direction = (player.position - head.position).normalized;
        float angle = Vector3.Angle(head.forward, direction);
        bool agentReady = AgentReady();
        #region Detection
        if (distance < detectionRadius || 
        (distance < veiwRadius &&
         angle  < veiwAngle / 2 &&
        (!raycast || !Physics.Raycast(head.position, direction, distance, enviromentMask))))
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
        if (agentReady)
        {
            rotaionTransform.rotation = Quaternion.RotateTowards(rotaionTransform.rotation, Quaternion.LookRotation(lastDirection) * Quaternion.Inverse(Quaternion.Euler(-90f, 0f, 180f)), turningSpeed * Time.deltaTime);
        }
        #endregion

        #region Debug
        #if UNITY_EDITOR
        Debug.DrawRay(head.position, direction * distance, Color.red);
        if (!agentReady)
            Debug.DrawRay(head.position, Vector3.up, Color.yellow);
        Debug.DrawRay(head.position,head.forward * 100,Color.green);
        Debug.DrawRay(rotaionTransform.position, Quaternion.Euler(-90, 0, 180) * rotaionTransform.forward * 100,Color.blue);
        #endif
        #endregion
    }
}