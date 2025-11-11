using UnityEngine;
using System.Collections;

////////////////////////////////////////////////////////////////
/// PLEASE NOTE: You must give you playable character the player tag,
/// and the terrain the environment layer otherwise this will not work.
////////////////////////////////////////////////////////////////
public class MobSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;        // The enemy to spawn
    public float spawnRadius = 10f;       // How far from player to spawn
    public float minSpawnDistance = 5f;   // Minimum distance from player
    public int maxEnemies = 5;           // Maximum enemies alive at once
    public float spawnInterval = 3f;      // Time between spawn attempts

    [Header("Ground Check")]
    public float maxGroundHeight = 100f;  // Maximum height to check for ground
    public LayerMask groundLayer;         // Layer(s) considered as ground

    private Transform player;
    private int currentEnemies;
    private bool isSpawning;

    private void Start()
    {
        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        isSpawning = true;
        while (isSpawning)
        {
            if (currentEnemies < maxEnemies)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        // Get random point in circle around player
        Vector2 randomCircle = Random.insideUnitCircle.normalized *
            Random.Range(minSpawnDistance, spawnRadius);

        // Convert to Vector3 position
        Vector3 spawnPos = player.position + new Vector3(randomCircle.x, 0, randomCircle.y);

        // Raycast down to find ground
        RaycastHit hit;
        if (Physics.Raycast(spawnPos + Vector3.up * maxGroundHeight, Vector3.down, out hit, maxGroundHeight * 2, groundLayer))
        {
            // Spawn the enemy at the ground position
            GameObject enemy = Instantiate(enemyPrefab, hit.point, Quaternion.identity);
            currentEnemies++;

            // Subscribe to enemy destruction to update count
            var destroyNotifier = enemy.AddComponent<DestroyNotifier>();
            destroyNotifier.OnDestroyed += OnEnemyDestroyed;
        }
    }

    private void OnEnemyDestroyed()
    {
        currentEnemies--;
    }

    private void OnDisable()
    {
        isSpawning = false;
    }
}

// Helper component to notify when an enemy is destroyed
public class DestroyNotifier : MonoBehaviour
{
    public System.Action OnDestroyed;

    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}
