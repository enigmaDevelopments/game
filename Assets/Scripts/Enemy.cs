using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;  // Health of the enemy
    private ChainReactionUpgrade chainReactionUpgrade;

    [System.Obsolete]
    private void Start()
    {
        chainReactionUpgrade = FindObjectOfType<ChainReactionUpgrade>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // If the player has the chain reaction upgrade, try triggering it
        if (chainReactionUpgrade != null)
        {
            chainReactionUpgrade.TryTriggerChainReaction(this);
        }

        // Destroy the enemy object
        Destroy(gameObject);
    }
}
