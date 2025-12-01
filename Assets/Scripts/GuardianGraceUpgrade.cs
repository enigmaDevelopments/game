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

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");  // update tag
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>(); //update script name
        }
    }

    private void Update()
    {
        if (!isActive || playerHealth == null) return;

        // Check health percentage
        float healthPercent = playerHealth.currentHealth / playerHealth.maxHealth;

        if (healthPercent <= triggerThreshold && canTrigger)
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

        // Grant invulnerability
        playerHealth.isInvulnerable = true;

        // Optional: Add visual or sound effects here
        yield return new WaitForSeconds(graceDuration);

        // End invulnerability
        playerHealth.isInvulnerable = false;

        Debug.Log("Guardian Grace ended.");

        // Start cooldown
        yield return new WaitForSeconds(cooldown);
        canTrigger = true;
    }
}
