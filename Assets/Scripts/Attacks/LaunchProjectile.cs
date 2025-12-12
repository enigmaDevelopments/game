using System.Collections;
using UnityEngine;

public class LaunchProjectile : AttackBase
{
    [Header("Projectile Settings")]
    public Rigidbody projectile;
    public float speed = 4f;
    public float duration;
    public Transform spawnTransform;
    public int layer;

    protected override IEnumerator ExecuteAttack()
    {
        if (projectile == null)
            yield break;
        if (animation != null)
            yield return StartCoroutine(animation.AttackingStance());
        if (spawnTransform == null)
            spawnTransform = transform;
        Shoot();
        // No additional timing needed; projectile is fired instantly.
        yield break;
    }

    protected virtual void Shoot()
    {
        Shoot(Quaternion.identity);
    }

    protected Projectile Shoot(Quaternion rotation)
    {
        Rigidbody p = Instantiate(projectile, spawnTransform.position, spawnTransform.rotation);
        p.linearVelocity = rotation * transform.forward * speed;
        Projectile projectileScript = p.GetComponent<Projectile>();
        projectileScript.duration = duration;
        projectileScript.owner = gameObject;
        projectileScript.damage = damage;
        p.gameObject.layer = layer;
        return projectileScript;
    } 
}


