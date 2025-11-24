using UnityEngine;
using System.Collections;

public class CriticalInstinctsUpgrade : MonoBehaviour
{
    public float criticalChance = 0.2f;  // Chance to deal a critical hit (20% by default)
    public float criticalDamageMultiplier = 2f;  // How much damage is increased on critical hits (e.g., 2x damage)
    public float duration = 5f;  // How long the critical instinct upgrade lasts (e.g., 5 seconds)
    private bool isUpgradeActive = false;  // Flag to check if the upgrade is active

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateCriticalInstincts();
            Destroy(gameObject);  // Destroy the upgrade item after it's picked up
        }
    }

    private void ActivateCriticalInstincts()
    {
        isUpgradeActive = true;
        Debug.Log("Critical Instincts Upgrade Activated!");

        // Automatically deactivate the upgrade after the duration ends
        StartCoroutine(DeactivateCriticalInstinctsAfterTime());
    }

    private IEnumerator DeactivateCriticalInstinctsAfterTime()
    {
        yield return new WaitForSeconds(duration);
        isUpgradeActive = false;
        Debug.Log("Critical Instincts Upgrade Expired.");
    }

    public bool IsUpgradeActive()
    {
        return isUpgradeActive;
    }

    public float GetCriticalChance()
    {
        return criticalChance;
    }

    public float GetCriticalDamageMultiplier()
    {
        return criticalDamageMultiplier;
    }
}
