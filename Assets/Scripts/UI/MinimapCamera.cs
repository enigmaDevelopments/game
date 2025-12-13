using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float height = 50f;  // Height above ground
    [SerializeField] private float orthographicSize = 50f;  // Zoom level

    private Camera minimapCameraComponent;

    private void Start()
    {
        minimapCameraComponent = GetComponent<Camera>();
        
        // Find player if not assigned
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                playerTransform = playerObj.transform;
        }

        // Setup orthographic camera
        if (minimapCameraComponent != null)
        {
            minimapCameraComponent.orthographic = true;
            minimapCameraComponent.orthographicSize = orthographicSize;
            minimapCameraComponent.clearFlags = CameraClearFlags.SolidColor;
            minimapCameraComponent.backgroundColor = Color.black;
            minimapCameraComponent.depth = -100;  // Render before everything
            // IMPORTANT: Don't set targetTexture here - MinimapDisplay does it
        }
    }

    /// <summary>
    /// Set the player transform for the minimap to follow
    /// </summary>
    public void SetPlayerTransform(Transform newPlayer)
    {
        playerTransform = newPlayer;
    }

    private void LateUpdate()
    {
        if (playerTransform == null || minimapCameraComponent == null)
            return;

        // Follow player from above
        Vector3 pos = playerTransform.position;
        transform.position = new Vector3(pos.x, height, pos.z);
        transform.LookAt(playerTransform.position);
    }
}
