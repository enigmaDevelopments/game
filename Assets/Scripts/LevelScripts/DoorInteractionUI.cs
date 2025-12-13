using UnityEngine;
using UnityEngine.UI;

public class DoorInteractionUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Canvas interactionCanvas;
    [SerializeField] private Text interactionText;
    [SerializeField] private string promptMessage = "Press E to Enter";

    [Header("Animation")]
    [SerializeField] private bool pulseEffect = true;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float pulseMin = 0.7f;
    [SerializeField] private float pulseMax = 1f;

    private void Start()
    {
        // Hide UI by default
        if (interactionCanvas != null)
            interactionCanvas.enabled = false;

        if (interactionText != null)
            interactionText.text = promptMessage;
    }

    private void Update()
    {
        // Pulse effect for UI
        if (pulseEffect && interactionCanvas != null && interactionCanvas.enabled)
        {
            float scale = Mathf.Lerp(pulseMin, pulseMax, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
            if (interactionText != null)
                interactionText.transform.localScale = Vector3.one * scale;
        }
    }

    public void ShowPrompt()
    {
        if (interactionCanvas != null)
            interactionCanvas.enabled = true;
    }

    public void HidePrompt()
    {
        if (interactionCanvas != null)
            interactionCanvas.enabled = false;
    }

    public void SetPromptMessage(string message)
    {
        if (interactionText != null)
            interactionText.text = message;
    }
}
