using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float health = 100f;  // Enemy health

    [Header("Coin Drop Settings")]
    public GameObject coinPrefab;        // assign your Coin prefab here
    public int coinsToDrop = 5;          // number of coins per enemy
    public float dropSpread = 0.5f;      // how far coins scatter

    private ChainReactionUpgrade chainReactionUpgrade;

    // Event fired when an enemy dies — upgrades or other systems can listen
    public static event Action<Enemy> OnEnemyDeath;

    private void Start()
    {
        chainReactionUpgrade = FindAnyObjectByType<ChainReactionUpgrade>();
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
