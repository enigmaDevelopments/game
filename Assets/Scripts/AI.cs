using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public bool runAway;
    public float runAwayRadius;
    public float detectionRadius;
    private NavMeshAgent agent;
    private Transform player;
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        if (!runAway)
            runAwayRadius = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, player.position) < detectionRadius)
            agent.SetDestination(player.position + runAwayRadius * (transform.position - player.position).normalized);
    }
}
