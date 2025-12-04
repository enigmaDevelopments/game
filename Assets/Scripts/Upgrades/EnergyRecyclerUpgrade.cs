using UnityEngine;

public class EnergyRecyclerUpgrade : MonoBehaviour
{
    [Header("Energy Restore Settings")]
    [Range(0f, 1f)] public float energyRestorePercent = 0.1f; // 10% of max energy
    public bool isActive = false;  // Will be set true when purchased

    private PlayerEnergy playerEnergy;

    private void Start()
    {
        // Subscribe to enemy death event
        Enemy.OnEnemyDeath += HandleEnemyDeath;

        // Find the player and get their energy component
        GameObject player = GameObject.FindGameObjectWithTag("Player");  // update with correct tag
        if (player != null)
        {
            playerEnergy = player.GetComponent<PlayerEnergy>();
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        Enemy.OnEnemyDeath -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        // Only do anything if the upgrade is active and the player has an energy component
        if (!isActive || playerEnergy == null) return;

        float restoreAmount = playerEnergy.maxEnergy * energyRestorePercent;
        playerEnergy.RestoreEnergy(restoreAmount);

        Debug.Log($"Energy Recycler activated! Restored {restoreAmount} energy.");
    }
}
