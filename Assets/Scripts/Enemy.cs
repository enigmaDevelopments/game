using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float health = 100f;  // Enemy health

    private ChainReactionUpgrade chainReactionUpgrade;

    // Event fired when an enemy dies � upgrades or other systems can listen
    public static event Action<Enemy> OnEnemyDeath;

    private void Start()
    {
        // Find upgrades if you want to trigger them directly
        chainReactionUpgrade = FindFirstObjectByType<ChainReactionUpgrade>();
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
        Debug.Log($"{gameObject.name} died!");

        // Trigger chain reaction upgrade if it's active
        // If the player has the chain reaction upgrade, try triggering it
        if (chainReactionUpgrade != null)
        {
            chainReactionUpgrade.TryTriggerChainReaction(this);
        }

        // Notify any listeners that this enemy has died (EnergyRecyclerUpgrade will handle its own logic)
        OnEnemyDeath?.Invoke(this);

        // Destroy the enemy
        // Destroy the enemy object
        Destroy(gameObject);
    }
}
