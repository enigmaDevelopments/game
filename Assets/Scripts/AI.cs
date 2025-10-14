using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    
    public float runAwayDistance;
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
            destnation = player.position + (transform.position - player.position).normalized * runAwayDistance;
        agent.SetDestination(destnation);
    }
}
