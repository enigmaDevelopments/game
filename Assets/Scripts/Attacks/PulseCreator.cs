using System.Collections;
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

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }
    protected override IEnumerator ExecuteAttack()
    {
        particles.Play();
        Collider[] enemies = Physics.OverlapSphere(transform.position, raduis, enemyMask);
        foreach (Collider enemy in enemies)
        {
            if (enemy.transform.root != enemy.transform)
                continue;
            Enable(enemy.gameObject, false);
            Rigidbody rb = enemy.GetComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.AddExplosionForce(force, transform.position, raduis, upwardsModifyer, ForceMode.Impulse);
            StartCoroutine(GetUp(enemy.gameObject));
        }
        yield break;
    }
    private IEnumerator GetUp(GameObject enemy)
    {
        float height = enemy.GetComponent<NavMeshAgent>().baseOffset;
        yield return null;
        do
            yield return new WaitForSeconds(proneTime);
        while (NavMesh.SamplePosition(enemy.transform.position, out NavMeshHit hit, height + 1, NavMesh.AllAreas));
        Enable(enemy);
        enemy.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
        yield break;
    }
    private void Enable(GameObject enemy, bool enable = true)
    {
        enemy.GetComponent<NavMeshAgent>().enabled = enable;
        foreach(Animation animation in enemy.GetComponents<Animation>())
            animation.enabled = enable;
    }
}