using System.Collections;
using UnityEngine;

public class LaunchProjectile : AttackBase
{
    [Header("Projectile Settings")]
    public Rigidbody projectile;
    public float speed = 4f;
    public Vector3 spawnOffset = new Vector3(0f, 0f, 0.5f);

    protected override IEnumerator ExecuteAttack()
    {
        if (projectile == null)
            yield break;

        Rigidbody p = Instantiate(projectile, transform.position + transform.TransformDirection(spawnOffset), transform.rotation);
        p.linearVelocity = transform.forward * speed;

        // No additional timing needed; projectile is fired instantly.
        yield break;
    }
}


