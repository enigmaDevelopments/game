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

    private void Awake()
    {
        id = Random.Range(int.MinValue, int.MaxValue);
    }
    protected virtual void Start()
    {
        // Deletes the projectile after duration seconds
        Destroy(gameObject, duration);
    }

    protected virtual void OnTriggerEnter(Collider collision)
    {
        #if UNITY_EDITOR
        GameObject defender = collision.gameObject;
        if (owner != null)
        {
            Debug.Log($"{owner.name} {attackName} hit {defender.name} id");
        }
        else
        {
            Debug.Log($"Projectile hit {defender.name}");
        }
        #endif

        // Apply damage if the thing hit has an Enemy component (check root and children)
        AttackBase.Damage(defender.transform, damage, id);

        // When the projectile hits something, create an explosion
        // and remove the projectile.
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        if (defender.layer == 10) 
            Debug.Log(true);
        Destroy(gameObject);
    }
}

