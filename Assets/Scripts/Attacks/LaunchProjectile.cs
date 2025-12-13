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
    public AimingSystem aim;

    private void Start()
    {
        if (spawnTransform == null)
            spawnTransform = transform;
        aim = transform.root.GetComponent<AimingSystem>();
        if (aim != null)
        {
            aim.weaponTransforms.Add(spawnTransform);
            aim.rotationTransforms.Add(transform.parent.parent.parent.parent);
        }
    }
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
        Rigidbody p = Instantiate(projectile, spawnTransform.position, spawnTransform.rotation);
        p.linearVelocity = rotation * transform.forward * speed;
        Projectile projectileScript = p.GetComponent<Projectile>();
        projectileScript.duration = duration;
        projectileScript.owner = gameObject;
        projectileScript.damage = currentDamage;
        p.gameObject.layer = layer;
        return projectileScript;
    }

    private void OnDestroy()
    {
        if (aim != null)
        {
            aim.weaponTransforms.Remove(spawnTransform);
            aim.rotationTransforms.Remove(transform.parent.parent.parent.parent);
        }
    }
}


