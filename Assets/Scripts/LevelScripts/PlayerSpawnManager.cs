using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Transform startDoorSpawn;
    [SerializeField] private Transform endDoorSpawn;
    [SerializeField] private Transform defaultSpawn;

    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("PlayerSpawnManager: Player not found in scene! Make sure your player has the 'Player' tag.");
            return;
        }

        // Get the last spawn point used from previous scene
        string lastSpawnPoint = PlayerPrefs.GetString("LastSpawnPoint", "");

        Transform spawnPoint = defaultSpawn;

        // Determine which spawn point to use
        if (lastSpawnPoint == "StartDoor" && startDoorSpawn != null)
        {
            spawnPoint = startDoorSpawn;
            if (showDebugInfo)
                Debug.Log("Player spawned at Start Door");
        }
        else if (lastSpawnPoint == "EndDoor" && endDoorSpawn != null)
        {
            spawnPoint = endDoorSpawn;
            if (showDebugInfo)
                Debug.Log("Player spawned at End Door");
        }
        else
        {
            if (showDebugInfo)
                Debug.Log("Player spawned at Default Spawn");
        }

        // Ensure we have a valid spawn point
        if (spawnPoint == null)
        {
            Debug.LogWarning("PlayerSpawnManager: No valid spawn point found! Player will spawn at (0,0,0)");
            spawnPoint = transform; // Use this manager's position as fallback
        }

        // Position player at spawn point
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            // Disable controller temporarily to prevent physics conflicts
            controller.enabled = false;
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;
            controller.enabled = true;
        }
        else
        {
            // No character controller, just set position directly
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;
        }

        if (showDebugInfo)
            Debug.Log($"Player positioned at: {spawnPoint.position}");
    }

    // Optional: Manually set spawn point from code
    public void SetSpawnPoint(string spawnPointName)
    {
        PlayerPrefs.SetString("LastSpawnPoint", spawnPointName);
    }

    // Optional: Clear spawn point data
    public void ClearSpawnData()
    {
        PlayerPrefs.DeleteKey("LastSpawnPoint");
    }

    // Visualize spawn points in editor
    private void OnDrawGizmos()
    {
        if (startDoorSpawn != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(startDoorSpawn.position, 0.5f);
            Gizmos.DrawLine(startDoorSpawn.position, startDoorSpawn.position + startDoorSpawn.forward * 2f);
        }

        if (endDoorSpawn != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(endDoorSpawn.position, 0.5f);
            Gizmos.DrawLine(endDoorSpawn.position, endDoorSpawn.position + endDoorSpawn.forward * 2f);
        }

        if (defaultSpawn != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(defaultSpawn.position, 0.5f);
            Gizmos.DrawLine(defaultSpawn.position, defaultSpawn.position + defaultSpawn.forward * 2f);
        }
    }
}
