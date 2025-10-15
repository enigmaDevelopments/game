using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public LayerMask enviromentMask;
    public bool runAway;
    public float runAwayRadius;
    public bool detection;
    public float detectionRadius;
    public bool seight;
    public bool raycast;
    public float veiwAngle;
    public float veiwRadius;
    private NavMeshAgent agent;
    private Transform player;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        if (!runAway)
            runAwayRadius = 0;
        if (!detection)
            detectionRadius = 0;
        if (!seight)
            veiwRadius = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 direction = (transform.position - player.position).normalized;
        if (distance < detectionRadius)
            agent.SetDestination(player.position + runAwayRadius * direction);
        else if(distance < veiwRadius)
            if (Vector3.Angle(transform.forward, -direction) < veiwAngle / 2 )
                if (!raycast || !Physics.Raycast(transform.position, -direction, distance, enviromentMask))
                    agent.SetDestination(player.position + runAwayRadius * direction);
       #if UNITY_EDITOR
       Debug.DrawRay(transform.position, -direction * distance, Color.red);
       #endif
    }
}