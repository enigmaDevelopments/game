using UnityEngine;
using System.Collections;

public class GuardianGraceUpgrade : MonoBehaviour
{
    [Header("Guardian Grace Settings")]
    [Tooltip("Health percentage threshold to trigger grace (e.g. 0.25 = 25%)")]
    [Range(0f, 1f)] public float triggerThreshold = 0.25f;

    [Tooltip("How long the grace effect lasts (in seconds)")]
    public float graceDuration = 3f;

    [Tooltip("How much health to restore (as a percentage of max health)")]
    [Range(0f, 1f)] public float healPercent = 0.3f;

    [Tooltip("Cooldown before Guardian Grace can trigger again (seconds)")]
    public float cooldown = 15f;

    [Tooltip("Enable or disable this upgrade")]
    public bool isActive = false;

    private PlayerHealth playerHealth;
    private bool canTrigger = true;

    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;

            // Find player reference
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<PlayerHealth>();
            }

            Debug.Log("Guardian Grace Upgrade Activated!");
        }
    }

    private void Update()
    {
        if (!isActive || playerHealth == null || !canTrigger) return;

        float healthPercent = playerHealth.currentHealth / playerHealth.maxHealth;
        if (healthPercent <= triggerThreshold)
        {
            StartCoroutine(ActivateGuardianGrace());
        }
    }

    private IEnumerator ActivateGuardianGrace()
    {
        canTrigger = false;

        Debug.Log("??? Guardian Grace Activated!");

        // Heal player
        float healAmount = playerHealth.maxHealth * healPercent;
        playerHealth.RestoreHealth(healAmount);

        // Grant temporary invulnerability
        playerHealth.isInvulnerable = true;

        yield return new WaitForSeconds(graceDuration);

        playerHealth.isInvulnerable = false;
        Debug.Log("Guardian Grace ended.");

        // Cooldown before it can trigger again
        yield return new WaitForSeconds(cooldown);
        canTrigger = true;
    }
}
