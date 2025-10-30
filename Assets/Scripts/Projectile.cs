using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject explosion;

    // Metadata passed from the launcher
    public GameObject owner;
    public string attackName;
    public float duration;

    void Start()
    {
        // Deletes the projectile after 10 seconds, regardless
        // of whether it collided with anything. 
        Destroy(gameObject, duration);
    }

    void OnCollisionEnter(Collision collision)
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

        // When the projectile hits something, create an explosion
        // and remove the projectile.
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}

