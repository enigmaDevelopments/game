using UnityEngine;

public class ChainReaction : MonoBehaviour
{
    public float explosionRadius = 5f;  // Radius in which enemies or objects are affected
    public float explosionDamage = 50f; // Amount of damage dealt in the chain reaction

    public void TriggerChainReaction()
    {
        // Find all objects in the explosion radius
        Collider2D[] affectedObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D collider in affectedObjects)
        {
            // Check if the object is an enemy (or any other type of target)
            if (collider.CompareTag("Enemy"))
            {
                // Apply damage to the enemy
                var enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(explosionDamage);
                    Debug.Log("Enemy hit by chain reaction!");
                }
            }
            // If there's a barrier, health packs, or other objects to trigger, add conditions here
            // For example, if there's a "Barrel" tag:
            else if (collider.CompareTag("Barrel"))
            {
                // Trigger an explosion on barrels (can destroy them or deal damage)
                /*var barrel = collider.GetComponent<Barrel>();
                if (barrel != null)
                {
                    barrel.Explode();
                }*/
            }
        }

        // Optionally, destroy the object that triggered the chain reaction (e.g., an enemy or barrel)
        Destroy(gameObject);
    }

    // Optional: Visualize the radius (helpful for debugging or testing)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
