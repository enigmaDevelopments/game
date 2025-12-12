using System.Collections;
using UnityEngine;

public class LaunchProjectile : AttackBase
{
    [Header("Projectile Settings")]
    public Rigidbody projectile;
    public float speed = 4f;
    public float duration;
    public Vector3 spawnOffset = new Vector3(0f, 0f, 0.5f);
    public int layer;

    protected override IEnumerator ExecuteAttack()
    {
        if (projectile == null)
            yield break;
        if (animation != null)
            yield return StartCoroutine(animation.AttackingStance());
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
        Rigidbody p = Instantiate(projectile, transform.position + transform.TransformDirection(spawnOffset), transform.rotation);
        p.linearVelocity = rotation * transform.forward * speed;
        Projectile projectileScript = p.GetComponent<Projectile>();
        projectileScript.duration = duration;
        projectileScript.owner = gameObject;
        projectileScript.damage = damage;
        p.gameObject.layer = layer;
        return projectileScript;
    } 
}


