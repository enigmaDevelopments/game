using UnityEngine;

public class SecondWindUpgrade : MonoBehaviour
{
    [Header("Second Wind Settings")]
    public bool isActive = false;            // If player bought the upgrade
    public float healthRestorePercent = 0.5f; // % of max health restored
    public float invulnerableDuration = 2f;   // Temporary invulnerability
    private bool hasTriggeredThisLife = false;

    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            PlayerHealth.OnPlayerDamaged += OnPlayerDamaged;
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            PlayerHealth.OnPlayerDamaged -= OnPlayerDamaged;
    }

    public void Activate()
    {
        isActive = true;
        Debug.Log("Second Wind Upgrade Activated!");
    }

    private void OnPlayerDamaged()
    {
        if (!isActive || hasTriggeredThisLife || playerHealth.currentHealth > 0)
            return;

        // Player would die, trigger second wind
        TriggerSecondWind();
    }

    private void TriggerSecondWind()
    {
        hasTriggeredThisLife = true;

        // Restore health
        float restoreAmount = playerHealth.maxHealth * healthRestorePercent;
        playerHealth.RestoreHealth(restoreAmount);

        // Temporarily make the player invulnerable
        StartCoroutine(TemporaryInvulnerability());

        // Optional: visual or sound feedback
        Debug.Log("? Second Wind Activated! Health restored.");
    }

    private System.Collections.IEnumerator TemporaryInvulnerability()
    {
        playerHealth.isInvulnerable = true;
        yield return new WaitForSeconds(invulnerableDuration);
        playerHealth.isInvulnerable = false;
    }

    // Call this when the player respawns or starts a new life
    public void ResetSecondWind()
    {
        hasTriggeredThisLife = false;
    }
}
