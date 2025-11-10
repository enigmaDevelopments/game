using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float baseDamage = 20f;  // Base damage of the player's attack
    private CriticalInstinctsUpgrade criticalUpgrade;  // Reference to the critical instincts upgrade

    [System.Obsolete]
    private void Start()
    {
        criticalUpgrade = FindObjectOfType<CriticalInstinctsUpgrade>();  // Find the upgrade (should be attached to a GameObject in the scene)
    }

    public void PerformAttack()
    {
        float damage = baseDamage;

        if (criticalUpgrade != null && criticalUpgrade.IsUpgradeActive())
        {
            // Check if a critical hit occurs based on the upgrade's chance
            if (Random.value <= criticalUpgrade.GetCriticalChance())  // Random.value generates a number between 0 and 1
            {
                // Apply critical damage
                damage *= criticalUpgrade.GetCriticalDamageMultiplier();
                Debug.Log("Critical Hit!");
            }
        }

        // Assuming you have a method to apply damage to enemies or objects
        ApplyDamageToEnemies(damage);
    }

    private void ApplyDamageToEnemies(float damage)
    {
        // Example damage application logic
        // You'd have some collision detection here to apply the damage to enemies in range
        Debug.Log($"Dealt {damage} damage to enemies!");
    }
}
