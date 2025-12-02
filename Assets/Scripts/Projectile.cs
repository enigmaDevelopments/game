using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject explosion;

    // Metadata passed from the launcher
    public GameObject owner;
    public string attackName;
    public float duration;

    // Damage applied when hitting an enemy
    public float damage = 10f;

    protected virtual void Start()
    {
        // Deletes the projectile after duration seconds
        Destroy(gameObject, duration);
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
            Debug.Log($"{owner.name} {attackName} hit {defender.name}");
        }
        else
        {
            Debug.Log($"Projectile hit {defender.name}");
        }

        // Apply damage if the thing hit has an Enemy component (check root and children)
        GameObject defenderRootObj = defender.transform.root.gameObject;
        Enemy enemy = defenderRootObj.GetComponent<Enemy>();
        if (enemy == null)
        {
            // try children if not on root
            enemy = defenderRootObj.GetComponentInChildren<Enemy>();
        }

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Apply damage if the thing hit has a Player tag and PlayerStats component
        if (defenderRootObj.CompareTag("Player"))
        {
            PlayerStats playerStats = defenderRootObj.GetComponent<PlayerStats>();
            if (playerStats == null)
            {
                // try children if not on root
                playerStats = defenderRootObj.GetComponentInChildren<PlayerStats>();
            }

            if (playerStats != null)
            {
                playerStats.TakeDamage((int)damage);
                
                // Check if player died (health <= 0)
                if (playerStats.currentHealth <= 0)
                {
                    Destroy(defenderRootObj);
                }
            }
        }

        // When the projectile hits something, create an explosion
        // and remove the projectile.
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}

