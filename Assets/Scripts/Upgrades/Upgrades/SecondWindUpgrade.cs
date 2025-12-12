using UnityEngine;
using System.Collections;

public class SecondWindUpgrade : MonoBehaviour
{
    public bool isActive = false;
    public float healthRestorePercent = 0.5f;
    public float invulnerableDuration = 2f;

    private bool hasTriggeredThisLife = false;
    private PlayerHealth playerHealth;

    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            playerHealth = FindAnyObjectByType<PlayerHealth>();
            if (playerHealth != null)
                PlayerHealth.OnPlayerDamaged += OnPlayerDamaged;

            Debug.Log("Second Wind Upgrade Activated!");
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            PlayerHealth.OnPlayerDamaged -= OnPlayerDamaged;
    }

    private void OnPlayerDamaged()
    {
        if (!isActive || hasTriggeredThisLife || playerHealth.currentHealth > 0)
            return;

        TriggerSecondWind();
    }

    private void TriggerSecondWind()
    {
        hasTriggeredThisLife = true;

        float restoreAmount = playerHealth.maxHealth * healthRestorePercent;
        playerHealth.RestoreHealth(restoreAmount);

        StartCoroutine(TemporaryInvulnerability());
        Debug.Log("Second Wind Activated!");
    }

    private IEnumerator TemporaryInvulnerability()
    {
        playerHealth.isInvulnerable = true;
        yield return new WaitForSeconds(invulnerableDuration);
        playerHealth.isInvulnerable = false;
    }

    public void ResetSecondWind()
    {
        hasTriggeredThisLife = false;
    }
}
