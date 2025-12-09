using System;
using UnityEngine;

// add to enemy script

public class Enemy : Health
{
    private ChainReactionUpgrade chainReactionUpgrade;

    // Event fired when an enemy dies — upgrades or other systems can listen
    public static event Action<Enemy> OnEnemyDeath;
    public event Action<Enemy> OnEnemyHit;

    protected override void Start()
    {
        base.Start();
        // Find upgrades if you want to trigger them directly
        chainReactionUpgrade = FindAnyObjectByType<ChainReactionUpgrade>();
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        OnEnemyHit?.Invoke(this);
    }

    protected override void Die()
    {
        #if UNITY_EDITOR
        Debug.Log($"{gameObject.name} died!");
        #endif
        // Trigger chain reaction upgrade if it's active
        if (chainReactionUpgrade != null)
        {
            chainReactionUpgrade.TryTriggerChainReaction(this);
        }

        // Notify any listeners that this enemy has died (EnergyRecyclerUpgrade will handle its own logic)
        OnEnemyDeath?.Invoke(this);

        // Destroy the enemy
        Destroy(gameObject);
    }
}
