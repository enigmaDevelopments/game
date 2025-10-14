using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
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
        agent.SetDestination(player.position);
    }
}
