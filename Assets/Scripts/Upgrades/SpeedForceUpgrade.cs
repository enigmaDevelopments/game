using UnityEngine;

public class SpeedForceUpgrade : MonoBehaviour
{
    [Header("Speed Force Settings")]
    public bool isActive = false;
    public float speedMultiplier = 1.2f;  // 20% speed boost

    private ThirdPersonMovement playerMovement;

    private void Start()
    {
        playerMovement = FindAnyObjectByType<ThirdPersonMovement>();

        if (isActive && playerMovement != null)
        {
            ApplySpeedBoost();
        }
    }

    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            ApplySpeedBoost();
            Debug.Log("? Speed Force Upgrade Activated!");
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
