using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    [Tooltip("Find and configure all cameras in the scene to follow the player")]
    [SerializeField] private bool autoConfigureCameras = true;

    private Transform playerTransform;

    private void Start()
    {
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("CameraSetup: Player not found with 'Player' tag!");
            return;
        }

        playerTransform = playerObj.transform;

        if (autoConfigureCameras)
        {
            ConfigureAllCameras();
        }
    }

    private void ConfigureAllCameras()
    {
        // All cameras in the scene auto-configure themselves in their Start() method
        // They find the player using the "Player" tag
        // No manual configuration needed - just make sure the player has the "Player" tag

        Debug.Log("CameraSetup: All cameras will auto-configure to follow the player");
    }
}
