using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.VisualScripting.FullSerializer;

public class AI : MonoBehaviour
{
    #if UNITY_EDITOR
        public bool sight;
        public bool runAway;
        public bool detection;
        public bool hasWeapon;
        public bool omniscient;
        public bool lookAtPlayer;
        public bool pitchRotation;
        public Vector3 offsetVector;
    #endif

    public LayerMask enviromentMask;
    public float runAwayRadius;
    public float detectionRadius;
    public bool raycast;
    public float veiwAngle;
    public float veiwRadius;
    public bool search;
    public Enemy health;
    public float turningSpeed;
    public AttackController attackController;
    public static bool playerInvisable = false;
    public Transform rotaionTransform;
    public Transform head;
    public Quaternion offsetAngle;
    public float pitchMaximum;
    public float checksPerSecond;

    private NavMeshAgent agent;
    private Transform player;
    private Vector3 lastDirection;
    private float timer;
    private bool playerDetected = false;
    private bool searching = false;

    private bool AgentReady()
    {
        return agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh;
    }
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        attackController = GetComponent<AttackController>();
        // Initialize directions to avoid zero-vector LookRotation
        lastDirection = head.forward;
        if (search)
            health.OnEnemyHit += Hit;
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
        timer += Time.fixedDeltaTime * checksPerSecond;
        #region Detection
        if (1 < timer)
        {
            timer %= 1;
            playerDetected = distance < detectionRadius ||
            (distance < veiwRadius &&
             angle < veiwAngle / 2 &&
            (!raycast || !Physics.Raycast(head.position, direction, distance, enviromentMask)));
            if (playerDetected && agentReady)
            {
                lastDirection = direction;
                if (agentReady)
                    agent.SetDestination(player.position + runAwayRadius * -direction);
            }

        }
        #endregion

        #region Turning
        if (agentReady && !searching)
        {
            LookAt(rotaionTransform, lastDirection, offsetAngle);
        }
        #endregion

        #region attack
        if (playerDetected)
            foreach (AttackController.AiSettings attack in attackController.attacks)
                if (attack.canUse && angle < attack.attackAngle / 2 && (attack.canAttackWhileMoving || (agentReady && agent.remainingDistance <= agent.stoppingDistance)))
                {
                    if (attack.rotationTransform != null && attack.weponTransform != null)
                        LookAt(attack.rotationTransform, (player.position - attack.weponTransform.position).normalized, attack.rotationOffset, false);
                    attack.attack.TryAttack();
                }
        #endregion

        #region Debug
#if UNITY_EDITOR
        Debug.DrawRay(head.position, direction * distance, Color.red);
        if (!agentReady)
            Debug.DrawRay(head.position, Vector3.up, Color.yellow);
        Debug.DrawRay(head.position,head.forward * 100,Color.green);
        Debug.DrawRay(rotaionTransform.position, offsetAngle * rotaionTransform.forward * 100,Color.blue);
        #endif
        #endregion
    }
    
    private void Hit(Enemy enemy)
    {
        StartCoroutine(Search());
    }
    private IEnumerator Search()
    {
        if (searching)
            yield break;
        searching = true;
        Vector3[] directions = new Vector3[4];
        Quaternion direction;
        directions[0] = (player.position - head.position).normalized;
        if (0 < Vector3.Dot(transform.right, directions[0]))
            direction = new Quaternion(0, 0.7071068f, 0, 0.7071068f);
        else
            direction = new Quaternion(0, -0.7071068f, 0, 0.7071068f);
        directions[1] = direction * directions[0];
        directions[2] = direction * directions[1];

        directions[3] = head.forward;

        for (int i = 0; i<4 && !playerDetected; i++)
        {
            for (Quaternion last = Quaternion.identity; last != rotaionTransform.rotation && !playerDetected;)
            {
                last = rotaionTransform.rotation;
                LookAt(rotaionTransform, directions[i], offsetAngle, false);
                yield return new WaitForFixedUpdate();
            }
        }
        searching = false;
        yield break;
    }
    private void LookAt(Transform rotaionTransform, Vector3 direction, Quaternion offsetAngle, bool clamp = true)
    {
        Quaternion lookAt = Quaternion.LookRotation(direction);
        if (clamp)
        {
            direction = lookAt.eulerAngles;
            direction.x = Mathf.Clamp(direction.x - (direction.x < 180 ? 0 : 360), -pitchMaximum, pitchMaximum);
            lookAt = Quaternion.Euler(direction);
        }
        rotaionTransform.rotation = Quaternion.RotateTowards(rotaionTransform.rotation, lookAt * offsetAngle, turningSpeed * Time.fixedDeltaTime);
    }
}