using UnityEngine;
using System.Collections;

public class CriticalInstinctsUpgrade : MonoBehaviour
{
    public bool isActive = false;
    public float criticalChance = 0.2f;
    public float criticalDamageMultiplier = 2f;
    public float duration = 5f;

    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            Debug.Log("Critical Instincts Upgrade Activated!");
            StartCoroutine(DeactivateAfterTime());
        }
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(duration);
        isActive = false;
        Debug.Log("Critical Instincts Upgrade Expired.");
    }
}
