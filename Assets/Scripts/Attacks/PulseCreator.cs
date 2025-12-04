using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PulseCreator : AttackBase
{
    [Min(0f)]
    public float raduis;
    [Min(0f)]
    public float force;
    public float upwardsModifyer;
    public float proneTime;
    [Header("Enemy Info")]
    public LayerMask enemyMask;

    private ParticleSystem particles;
    private List<GameObject> pushed = new List<GameObject>();

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }
    private void FixedUpdate()
    {
        foreach (GameObject enemy in pushed)
        {
            if (NavMesh.SamplePosition(enemy.transform.position, out NavMeshHit hit, 1, NavMesh.AllAreas))
                StartCoroutine(GetUp(enemy));

        }
    }
    protected override IEnumerator ExecuteAttack()
    {
        particles.Play();
        Collider[] enemies = Physics.OverlapSphere(transform.position, raduis, enemyMask);
        foreach (Collider enemy in enemies)
        {
            enemy.GetComponent<NavMeshAgent>().enabled = false;
            Rigidbody rb = enemy.GetComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.AddExplosionForce(force, transform.position, raduis, upwardsModifyer, ForceMode.Impulse);
            pushed.Add(enemy.gameObject);

        }
        yield break;
    }
    private IEnumerator GetUp(GameObject enemy)
    {
        yield return new WaitForSeconds(proneTime);
        if (!NavMesh.SamplePosition(enemy.transform.position, out NavMeshHit hit, 1, NavMesh.AllAreas))
            yield break;
        enemy.GetComponent<NavMeshAgent>().enabled = true;
        enemy.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
        pushed.Remove(enemy);
        yield break;
    }
}
