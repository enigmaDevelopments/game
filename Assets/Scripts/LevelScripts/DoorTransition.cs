using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DoorTransition : MonoBehaviour
{
    [Header("Scene Configuration")]
    [Tooltip("Drag the scene asset to load when player enters this door")]
#if UNITY_EDITOR
    [SerializeField] private SceneAsset targetScene;
#endif
    [SerializeField] private string targetSceneName;

    [Header("Door Type")]
    [SerializeField] private bool isStartDoor = false;
    [SerializeField] private bool isEndDoor = true;

    [Header("Visual Feedback (Optional)")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private bool requiresInteraction = false;

    private bool playerInRange = false;

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Auto-update scene name when scene asset is assigned
        if (targetScene != null)
        {
            targetSceneName = targetScene.name;
        }
    }
#endif

    private void Start()
    {
        // Hide interaction prompt by default
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    private void Update()
    {
        // If interaction is required, check for key press
        if (requiresInteraction && playerInRange && Input.GetKeyDown(interactionKey))
        {
            LoadNextScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            // Show interaction prompt if needed
            if (interactionPrompt != null && requiresInteraction)
                interactionPrompt.SetActive(true);

            // Auto-load if no interaction is required
            if (!requiresInteraction)
            {
                LoadNextScene();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // Hide interaction prompt
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
        }
    }

    private void LoadNextScene()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError($"Target scene is not set on door: {gameObject.name}");
            return;
        }

        Debug.Log($"Loading scene: {targetSceneName}");
        SceneManager.LoadScene(targetSceneName);
    }

    // Optional: For debugging in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = isEndDoor ? Color.green : Color.blue;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 2f);
    }
}
