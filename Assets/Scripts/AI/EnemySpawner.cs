using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns enemies based on distance from player, total enemy count, and player line of sight.
/// Prevents spawning when the player is looking at the spawn point.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableEnemy
    {
        [Tooltip("The enemy prefab to spawn")]
        public GameObject enemyPrefab;

        [Tooltip("Minimum distance from player to spawn this enemy")]
        public float minSpawnDistance = 5f;

        [Tooltip("Maximum distance from player to spawn this enemy")]
        public float maxSpawnDistance = 20f;

        [Tooltip("Weight for spawn chance (higher = more likely to spawn)")]
        [Range(0f, 1f)]
        public float spawnWeight = 1f;
    }

    [Header("Spawn Settings")]
    [SerializeField] private List<SpawnableEnemy> spawnableEnemies = new List<SpawnableEnemy>();
    
    [Tooltip("Time between spawn attempts")]
    [SerializeField] private float spawnInterval = 5f;

    [Tooltip("Maximum number of enemies that can exist in the scene")]
    [SerializeField] private int maxEnemyCount = 10;

    [Tooltip("Random spawn radius around the spawner position")]
    [SerializeField] private float spawnRadius = 2f;

    [Header("Player Detection Settings")]
    [Tooltip("Tag used to find the player")]
    [SerializeField] private string playerTag = "Player";

    [Tooltip("Field of view angle for player's vision (in degrees)")]
    [SerializeField] private float playerFOV = 60f;

    [Tooltip("Distance threshold for checking if player can see spawn point")]
    [SerializeField] private float playerViewDistance = 30f;

    [Tooltip("Layer mask for obstacles that block line of sight")]
    [SerializeField] private LayerMask obstacleLayerMask;

    [Header("Spawn Chance Modifiers")]
    [Tooltip("Base spawn chance when at max enemies (0-1)")]
    [SerializeField] private float minSpawnChance = 0.1f;

    [Tooltip("Max spawn chance when no enemies exist (0-1)")]
    [SerializeField] private float maxSpawnChance = 0.9f;

    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;
    [SerializeField] private bool showVisibilityLogs = true;
    [SerializeField] private float visibilityLogInterval = 1f;

    // Runtime tracking
    private Transform playerTransform;
    private Camera playerCamera;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private float spawnTimer;
    private float visibilityLogTimer;

    private void Start()
    {
        // Find the player
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError($"EnemySpawner: No GameObject with tag '{playerTag}' found!");
        }

        // Get the main camera (which Cinemachine controls)
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            Debug.LogError("EnemySpawner: No main camera found!");
        }

        // Start spawning coroutine
        StartCoroutine(SpawnRoutine());
    }

    private void Update()
    {
        // Clean up null references from destroyed enemies
        spawnedEnemies.RemoveAll(enemy => enemy == null);

        // Periodic visibility logging for debugging
        if (showVisibilityLogs)
        {
            visibilityLogTimer += Time.deltaTime;
            if (visibilityLogTimer >= visibilityLogInterval)
            {
                visibilityLogTimer = 0f;
                LogVisibilityStatus();
            }
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Try to spawn an enemy
            TrySpawnEnemy();
        }
    }

    private void TrySpawnEnemy()
    {
        if (playerTransform == null || spawnableEnemies.Count == 0)
            return;

        // Check if we've reached max enemy count
        if (spawnedEnemies.Count >= maxEnemyCount)
        {
            if (showDebugInfo)
                Debug.Log($"EnemySpawner: Max enemy count ({maxEnemyCount}) reached. Skipping spawn.");
            return;
        }

        // Check if the spawner itself is visible to the player
        if (IsSpawnerVisible())
        {
            if (showDebugInfo)
                Debug.Log("EnemySpawner: Spawner is visible to player. Spawn cancelled.");
            return;
        }

        // Calculate spawn chance based on current enemy count
        float enemyCountRatio = (float)spawnedEnemies.Count / maxEnemyCount;
        float spawnChance = Mathf.Lerp(maxSpawnChance, minSpawnChance, enemyCountRatio);

        // Roll for spawn
        if (Random.value > spawnChance)
        {
            if (showDebugInfo)
                Debug.Log($"EnemySpawner: Spawn chance failed ({spawnChance:P0})");
            return;
        }

        // Select a random enemy type based on weights
        SpawnableEnemy selectedEnemy = SelectRandomEnemy();
        if (selectedEnemy == null || selectedEnemy.enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner: No valid enemy selected for spawning.");
            return;
        }

        // Calculate spawn position based on player distance requirements
        Vector3 spawnPosition = CalculateSpawnPosition(selectedEnemy);

        // Check if player is looking at the spawn point
        if (IsPlayerLookingAtPosition(spawnPosition))
        {
            if (showDebugInfo)
                Debug.Log("EnemySpawner: Player is looking at spawn point. Spawn cancelled.");
            return;
        }

        // Spawn the enemy
        GameObject spawnedEnemy = Instantiate(selectedEnemy.enemyPrefab, spawnPosition, Quaternion.identity);
        spawnedEnemies.Add(spawnedEnemy);

        if (showDebugInfo)
            Debug.Log($"EnemySpawner: Spawned {selectedEnemy.enemyPrefab.name} at {spawnPosition}. Total enemies: {spawnedEnemies.Count}");
    }

    private SpawnableEnemy SelectRandomEnemy()
    {
        if (spawnableEnemies.Count == 0)
            return null;

        // Calculate total weight
        float totalWeight = 0f;
        foreach (var enemy in spawnableEnemies)
        {
            if (enemy.enemyPrefab != null)
                totalWeight += enemy.spawnWeight;
        }

        if (totalWeight <= 0f)
            return spawnableEnemies[0];

        // Select random enemy based on weight
        float randomValue = Random.value * totalWeight;
        float currentWeight = 0f;

        foreach (var enemy in spawnableEnemies)
        {
            if (enemy.enemyPrefab != null)
            {
                currentWeight += enemy.spawnWeight;
                if (randomValue <= currentWeight)
                    return enemy;
            }
        }

        return spawnableEnemies[0];
    }

    private Vector3 CalculateSpawnPosition(SpawnableEnemy enemy)
    {
        // Get distance from player
        float playerDistance = Vector3.Distance(transform.position, playerTransform.position);

        // Determine spawn distance based on enemy's min/max range
        float targetDistance = Random.Range(enemy.minSpawnDistance, enemy.maxSpawnDistance);

        // Calculate direction from player
        Vector3 directionFromPlayer = (transform.position - playerTransform.position).normalized;

        // If spawner is too close to player, push spawn point away
        Vector3 basePosition;
        if (playerDistance < targetDistance)
        {
            basePosition = playerTransform.position + directionFromPlayer * targetDistance;
        }
        else
        {
            basePosition = transform.position;
        }

        // Add random offset within spawn radius
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = basePosition + new Vector3(randomCircle.x, 0f, randomCircle.y);

        // Ensure spawn position is on the ground (raycast down)
        if (Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f))
        {
            spawnPosition.y = hit.point.y;
        }

        return spawnPosition;
    }

    private bool IsSpawnerVisible()
    {
        if (playerTransform == null || playerCamera == null)
            return false;

        Vector3 directionToSpawner = transform.position - playerCamera.transform.position;
        float distanceToSpawner = directionToSpawner.magnitude;

        // If spawner is too far, player can't see it anyway
        if (distanceToSpawner > playerViewDistance)
            return false;

        // Use camera's actual viewport position check for more accurate detection
        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(transform.position);
        
        // Check if the spawner is in front of the camera and within the viewport bounds
        if (viewportPoint.z < 0 || viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
        {
            // Spawner is not visible in viewport
            return false;
        }

        // Get the actual camera forward direction
        Vector3 cameraForward = playerCamera.transform.forward;

        // Calculate angle between camera's look direction and spawner
        float angleToSpawner = Vector3.Angle(cameraForward, directionToSpawner);

        // Check if within field of view
        if (angleToSpawner > playerFOV / 2f)
            return false;

        if (obstacleLayerMask.value != 0)
        {
            Vector3 rayOrigin = playerCamera.transform.position;
            if (Physics.Raycast(rayOrigin, directionToSpawner.normalized, 
                out RaycastHit hit, distanceToSpawner, obstacleLayerMask))
            {
                // Something is blocking the view - spawner is NOT visible
                return false;
            }
        }

        // Player can see the spawner (it's in viewport, within FOV, and nothing blocking)
        return true;
    }

    private bool IsPlayerLookingAtPosition(Vector3 position)
    {
        if (playerTransform == null || playerCamera == null)
            return false;

        Vector3 directionToSpawn = position - playerCamera.transform.position;
        float distanceToSpawn = directionToSpawn.magnitude;

        // If spawn point is too far player cant see it anyway
        if (distanceToSpawn > playerViewDistance)
            return false;

        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(position);
        
        if (viewportPoint.z < 0 || viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
        {
            return false;
        }

        // Get the actual camera forward direction
        Vector3 cameraForward = playerCamera.transform.forward;

        // Calculate angle between camera's look direction and spawn point
        float angleToSpawn = Vector3.Angle(cameraForward, directionToSpawn);

        // Check if within field of view
        if (angleToSpawn > playerFOV / 2f)
            return false;

        if (obstacleLayerMask.value != 0)
        {
            Vector3 rayOrigin = playerCamera.transform.position;
            if (Physics.Raycast(rayOrigin, directionToSpawn.normalized, 
                out RaycastHit hit, distanceToSpawn, obstacleLayerMask))
            {
                // Something is blocking the view
                return false;
            }
        }

        // Player is looking at the spawn point
        return true;
    }

    private void LogVisibilityStatus()
    {
        if (playerTransform == null || playerCamera == null)
            return;

        Vector3 directionToSpawner = transform.position - playerCamera.transform.position;
        float distanceToSpawner = directionToSpawner.magnitude;
        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(transform.position);
        Vector3 cameraForward = playerCamera.transform.forward;
        float angleToSpawner = Vector3.Angle(cameraForward, directionToSpawner);

        bool tooFar = distanceToSpawner > playerViewDistance;
        bool inViewportBounds = viewportPoint.z > 0 && viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1;
        bool withinFOV = angleToSpawner <= playerFOV / 2f;
        
        bool spawnerVisible = IsSpawnerVisible();

        string visibilityInfo = $"[{gameObject.name}] Visibility Status:\n" +
            $"  - FINAL RESULT: {(spawnerVisible ? "YES (BLOCKED - CANNOT SPAWN)" : "NO (CAN SPAWN)")}\n";
       

        Debug.Log(visibilityInfo);
    }

    public void ForceSpawn()
    {
        TrySpawnEnemy();
    }
    public int GetCurrentEnemyCount()
    {
        spawnedEnemies.RemoveAll(enemy => enemy == null);
        return spawnedEnemies.Count;
    }
    public void ClearAllEnemies()
    {
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        spawnedEnemies.Clear();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Draw spawn radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        // Draw spawnable enemy ranges
        if (playerTransform != null && spawnableEnemies.Count > 0)
        {
            foreach (var enemy in spawnableEnemies)
            {
                // Min distance
                Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
                Gizmos.DrawWireSphere(playerTransform.position, enemy.minSpawnDistance);

                // Max distance
                Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
                Gizmos.DrawWireSphere(playerTransform.position, enemy.maxSpawnDistance);
            }
        }

        // Draw player view distance
        if (playerTransform != null)
        {
            Gizmos.color = new Color(0f, 0f, 1f, 0.2f);
            Gizmos.DrawWireSphere(playerTransform.position, playerViewDistance);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw more detailed view cone when selected
        if (playerTransform != null && playerCamera != null)
        {
            Gizmos.color = Color.cyan;
            Vector3 playerForward = playerCamera.transform.forward;
            
            // Draw view cone
            Vector3 leftBoundary = Quaternion.Euler(0, -playerFOV / 2f, 0) * playerForward * playerViewDistance;
            Vector3 rightBoundary = Quaternion.Euler(0, playerFOV / 2f, 0) * playerForward * playerViewDistance;
            
            Gizmos.DrawLine(playerTransform.position, playerTransform.position + leftBoundary);
            Gizmos.DrawLine(playerTransform.position, playerTransform.position + rightBoundary);
        }
    }
#endif
}
