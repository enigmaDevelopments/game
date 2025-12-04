using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public bool isInvulnerable = false;

    // Event that fires whenever the player takes damage
    public static event System.Action OnPlayerDamaged;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isInvulnerable) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Notify other systems (e.g., MomentumUpgrade) that the player took damage
        OnPlayerDamaged?.Invoke();

        Debug.Log($"Player took {amount} damage. Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void RestoreHealth(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"Player healed for {amount}. Health: {currentHealth}/{maxHealth}");
    }

    private void Die()
    {
        Debug.Log("?? Player Died!");
        // Add respawn, game over, or Guardian Grace triggers here
    }
}
