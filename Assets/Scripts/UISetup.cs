using UnityEngine;

public class UISetup : MonoBehaviour
{
    public static UISetup Instance { get; private set; }
    
    [Header("UI References")]
    public Canvas mainCanvas;
    public HealthBar healthBar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupUI()
    {
        // Ensure canvas is set to screen space overlay
        if (mainCanvas != null)
        {
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            // Setup health bar position if it exists
            if (healthBar != null)
            {
                RectTransform healthBarRect = healthBar.GetComponent<RectTransform>();
                if (healthBarRect != null)
                {
                    // Set anchors to top left
                    healthBarRect.anchorMin = new Vector2(0, 1);
                    healthBarRect.anchorMax = new Vector2(0, 1);
                    healthBarRect.pivot = new Vector2(0, 1);
                    
                    // Position from top-left corner (adjust these values as needed)
                    healthBarRect.anchoredPosition = new Vector2(20, -20);
                }
            }
        }
    }
}