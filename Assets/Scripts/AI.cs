using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    #if UNITY_EDITOR
        public bool sight;
        public bool runAway;
        public bool detection;
    #endif

    public LayerMask enviromentMask;
    public float runAwayRadius;
    public float detectionRadius;
    
    public bool raycast;
    public float veiwAngle;
    public float veiwRadius;
    public float turningSpeed;
    private NavMeshAgent agent;
    private Transform player;
    private Rigidbody rb;
    private float distance;
    private Vector3 direction;
    private Vector3 lastSelfDirection;
    private Vector3 lastDirection;
    private float turningTime;
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distance = Vector3.Distance(transform.position, player.position);
        direction = (player.position - transform.position).normalized;

        if (distance < detectionRadius)
            OnDetectPlayer();
        else if ((distance < veiwRadius) &&
        (Vector3.Angle(transform.forward, direction) < veiwAngle / 2) &&
        (!raycast || !Physics.Raycast(transform.position, direction, distance, enviromentMask)))
            OnDetectPlayer();
        if (agent.remainingDistance <= agent.stoppingDistance && turningTime < 1)
        {
            turningTime += turningSpeed;
            rb.MoveRotation(Quaternion.Lerp(Quaternion.LookRotation(lastSelfDirection), Quaternion.LookRotation(lastDirection), turningTime));
        }
        else
        {
            turningTime = 0;
            lastSelfDirection = transform.forward;
        }



        #if UNITY_EDITOR
            Debug.DrawRay(transform.position, direction * distance, Color.red);
        #endif
    }
    private void OnDetectPlayer()
    {  
        lastDirection = direction;
        agent.SetDestination(player.position + runAwayRadius * -direction);
        if (agent.remainingDistance <= agent.stoppingDistance)
            rb.MoveRotation(Quaternion.LookRotation(direction));

    }
}