using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    
    public float runAwayDistance;
    public float detectionRadius;
    private NavMeshAgent agent;
    private Transform player;
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, player.position) < detectionRadius)
            agent.SetDestination(player.position + runAwayDistance * (transform.position - player.position).normalized);
    }
}
