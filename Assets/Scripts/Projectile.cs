using UnityEngine;

public class Projectile : MonoBehaviour
{

    public GameObject explosion;
    void Start()
    {
        // Deletes the projectile after 10 seconds, regardless
        // of whether it collided with anything. 
        Destroy(gameObject, 10);
    }
    void OnCollisionEnter()
    {
        // When the projectile hits something, create an explosion
        // and remove the projectile.
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}

