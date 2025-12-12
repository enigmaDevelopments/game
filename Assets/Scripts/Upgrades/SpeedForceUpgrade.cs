using UnityEngine;

public class SpeedForceUpgrade : MonoBehaviour
{
    [Header("Speed Force Settings")]
    public bool isActive = false;
    public float speedMultiplier = 1.2f;  // 20% speed boost

    private ThirdPersonMovement playerMovement;

    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;

            if (playerMovement == null)
                playerMovement = FindAnyObjectByType<ThirdPersonMovement>();

            ApplySpeedBoost();
            Debug.Log("Speed Force Upgrade Activated!");
        }
    }

    private void ApplySpeedBoost()
    {
        if (playerMovement != null)
        {
            playerMovement.maxSpeed *= speedMultiplier;
        }
    }
}
