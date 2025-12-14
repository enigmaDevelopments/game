using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] playerPrefabs; // same order as menu: 0,1,2
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int defaultIndex = 0;       // default if something goes weird

    void Start()
    {
        int index = PlayerSelection.HasSelection
            ? PlayerSelection.SelectedIndex   // user picked one
            : defaultIndex;                   // user never picked ? use default (0)

        if (index < 0 || index >= playerPrefabs.Length)
        {
            Debug.LogWarning("PlayerSpawner: index out of range, using default.");
            index = defaultIndex;
        }

        GameObject prefabToSpawn = playerPrefabs[index];
        Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

        Debug.Log("PlayerSpawner: spawned prefab index " + index + " (" + prefabToSpawn.name + ")");

        // after Instantiate(...)
        GameObject player = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

        var camController = FindAnyObjectByType<CameraController>();
        if (camController != null)
        {
            camController.SetTarget(player.transform);
            Debug.Log("PlayerSpawner: Camera target set to " + player.name);
        }
        else
        {
            Debug.LogWarning("PlayerSpawner: No CameraController found in scene.");
        }

    }
}
