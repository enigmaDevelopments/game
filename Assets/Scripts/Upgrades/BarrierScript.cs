using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    [Header("Barrier Stats")]
    public float maxBarrier = 100f;
    private float currentBarrier;

    [Header("Barrier Regen")]
    public float rechargeRate = 5f;
    private float regenCooldown = 1f;
    private float regenTimer;

    private bool isBarrierActive = false;
    public bool IsBarrierActive => isBarrierActive;

    private void Start()
    {
        currentBarrier = maxBarrier;
    }

    private void Update()
    {
        if (isBarrierActive && currentBarrier < maxBarrier)
        {
            regenTimer -= Time.deltaTime;

            if (regenTimer <= 0f)
            {
                currentBarrier = Mathf.Min(currentBarrier + rechargeRate, maxBarrier);
                regenTimer = regenCooldown;
            }
        }
    }


    // -------------------------------
    // BARRIER CONTROL
    // -------------------------------

    public void ActivateBarrier()
    {
        isBarrierActive = true;
        currentBarrier = maxBarrier;
        regenTimer = regenCooldown;   // reset regen timing
        Debug.Log("Barrier Activated!");
    }

    public void DeactivateBarrier()
    {
        isBarrierActive = false;
        Debug.Log("Barrier Deactivated.");
    }

    public void ClampBarrier()
    {
        currentBarrier = Mathf.Min(currentBarrier, maxBarrier);
    }

    // -------------------------------
    // DAMAGE HANDLING
    // -------------------------------

    public bool TakeDamage(float damage)
    {
        if (isBarrierActive)
        {
            currentBarrier -= damage;

            if (currentBarrier <= 0f)
            {
                currentBarrier = 0f;
                isBarrierActive = false;   // barrier breaks
            }

            return true; // barrier absorbed it
        }

        return false; // no barrier
    }

    public float GetCurrentBarrier() => currentBarrier;
}
