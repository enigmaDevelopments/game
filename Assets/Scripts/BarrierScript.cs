using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    public float maxBarrier = 100f;  // Maximum barrier strength
    private float currentBarrier;    // Current barrier strength
    private bool isBarrierActive = false;

    public float rechargeRate = 5f; // Rate at which the barrier recharges
    private float barrierRegenCooldown = 1f;  // Delay between regen ticks

    public bool IsBarrierActive => isBarrierActive;

    private void Start()
    {
        currentBarrier = maxBarrier;
    }

    private void Update()
    {
        if (isBarrierActive && currentBarrier < maxBarrier)
        {
            // Recharge the barrier over time when it's active and not full
            barrierRegenCooldown -= Time.deltaTime;
            if (barrierRegenCooldown <= 0f)
            {
                currentBarrier = Mathf.Min(currentBarrier + rechargeRate, maxBarrier);
                barrierRegenCooldown = 1f;
            }
        }
    }

    public bool TakeDamage(float damage)
    {
        if (isBarrierActive)
        {
            currentBarrier -= damage;

            if (currentBarrier <= 0f)
            {
                // Barrier is depleted, deactivate it
                isBarrierActive = false;
                currentBarrier = 0f;
            }

            return true;  // Barrier absorbed the damage
        }

        return false; // No barrier, damage goes to player health
    }

    public void ActivateBarrier()
    {
        isBarrierActive = true;
        currentBarrier = maxBarrier;  // Fully recharge the barrier when activated
        Debug.Log("Barrier Surge Activated!");
    }

    public void DeactivateBarrier()
    {
        isBarrierActive = false;
        Debug.Log("Barrier Surge Deactivated.");
    }

    public float GetCurrentBarrier() => currentBarrier;
}
