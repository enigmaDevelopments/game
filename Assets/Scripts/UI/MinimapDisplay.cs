using UnityEngine;
using UnityEngine.UI;

public class MinimapDisplay : MonoBehaviour
{
    [SerializeField] private RawImage minimapImage;
    [SerializeField] private MinimapCamera minimapCamera;
    [SerializeField] private int textureWidth = 256;
    [SerializeField] private int textureHeight = 256;

    private RenderTexture renderTexture;

    private void Start()
    {
        if (minimapImage == null)
        {
            Debug.LogError("MinimapDisplay: minimapImage not assigned!");
            return;
        }

        // Auto-find MinimapCamera if not assigned
        if (minimapCamera == null)
            minimapCamera = FindObjectOfType<MinimapCamera>();

        if (minimapCamera == null)
        {
            Debug.LogError("MinimapDisplay: MinimapCamera not found in scene!");
            return;
        }

        // Create render texture
        renderTexture = new RenderTexture(textureWidth, textureHeight, 24);
        renderTexture.name = "MinimapTexture";

        // Setup minimap camera
        Camera minimapCam = minimapCamera.GetComponent<Camera>();
        if (minimapCam != null)
        {
            // Render only to the texture, NOT to the screen
            minimapCam.targetTexture = renderTexture;
            minimapCam.enabled = true;
        }

        // Display on RawImage
        minimapImage.texture = renderTexture;

        Debug.Log("Minimap initialized! Check the corner for the minimap view.");
    }

    private void OnDestroy()
    {
        if (renderTexture != null)
            renderTexture.Release();
    }
}
