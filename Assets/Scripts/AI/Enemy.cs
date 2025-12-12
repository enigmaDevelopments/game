using System;
using UnityEngine;
// add to enemy script

public class Enemy : Health
{
    [Header("Enemy Settings")]
    //public float health = 100f;  // Enemy health

    [Header("Coin Drop Settings")]
    public GameObject coinPrefab;        // assign your Coin prefab here
    public int coinsToDrop = 5;          // number of coins per enemy
    public float dropSpread = 0.5f;      // how far coins scatter

    private ChainReactionUpgrade chainReactionUpgrade;

    // Event fired when an enemy dies — upgrades or other systems can listen
    public static event Action<Enemy> OnEnemyDeath;
    public event Action<Enemy> OnEnemyHit;

    protected override void Start()
    {
        base.Start();
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

        // Spawn coins BEFORE the enemy is destroyed
        SpawnCoins();

        // Notify any listeners
        OnEnemyDeath?.Invoke(this);

        // Destroy the enemy
        Destroy(gameObject);
    }

    private void SpawnCoins()
    {
        if (coinPrefab == null)
        {
            Debug.LogWarning("Enemy has no coinPrefab assigned!");
            return;
        }

        for (int i = 0; i < coinsToDrop; i++)
        {
            // random scatter around the enemy
            Vector3 offset = UnityEngine.Random.insideUnitSphere * dropSpread;
            offset.y = Mathf.Abs(offset.y) + 0.2f; // ensure coins spawn above the ground

            GameObject coin = Instantiate(
                coinPrefab,
                transform.position + offset,
                Quaternion.identity
            );

            // OPTIONAL: give coins a little pop/bounce
            Rigidbody rb = coin.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 force = UnityEngine.Random.insideUnitSphere * 2f;
                force.y = Mathf.Abs(force.y) + 1f;
                rb.AddForce(force, ForceMode.Impulse);
            }
        }
    }
}
