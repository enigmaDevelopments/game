using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    
    public bool runAway = false;
    public float stoppingDistance;
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
        Vector3 destnation;
        if (runAway)
            destnation = player.position + (transform.position - player.position).normalized * stoppingDistance;
        else
            destnation = player.position;
        agent.SetDestination(destnation);
    }
}
