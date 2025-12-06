using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject explosion;

    // Metadata passed from the launcher
    public GameObject owner;
    public string attackName;
    public float duration;
    public int id;

    // Damage applied when hitting an enemy
    public float damage = 10f;

    protected virtual void awake()
    {
        // Deletes the projectile after duration seconds
        Destroy(gameObject, duration);
        id = Random.Range(int.MinValue, int.MaxValue);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // Ignore collisions with the entity that launched this projectile (including its children)
        GameObject defender = collision.rigidbody != null ? collision.rigidbody.gameObject : collision.gameObject;
        if (owner != null)
        {
            var ownerRoot = owner.transform.root;
            var defenderRoot = defender.transform.root;
            if (defenderRoot == ownerRoot)
            {
                return; // return early before any logging or effects
            }
        }

        if (owner != null)
        {
            Debug.Log($"{owner.name} {attackName} hit {defender.name} id {id}");
        }
        else
        {
            Debug.Log($"Projectile hit {defender.name}");
        }

        // Apply damage if the thing hit has an Enemy component (check root and children)
        GameObject defenderRootObj = defender.transform.root.gameObject;
        Health health = defenderRootObj.GetComponent<Health>();
        if (health != null)
            health.TakeDamage(damage, id);

        // When the projectile hits something, create an explosion
        // and remove the projectile.
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}

