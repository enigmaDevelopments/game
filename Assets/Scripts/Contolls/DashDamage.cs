using UnityEngine;

public class DashThroughDamage : PlayerDash
{
    public IntangibilityManager.FlashType flashType = IntangibilityManager.FlashType.visable;
    [Header("Damage Settings")]
    public int dashDamage = 10; // how much damage to deal per dash hit
    public float hitRadius = 0.5f;
    [Header("Layer Settings")]
    public LayerMask hitMask;
    private IntangibilityManager intangibilityManager;

    protected override void Start()
    {
        base.Start();
        intangibilityManager = GetComponent<IntangibilityManager>();
    }

    protected override System.Collections.IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        // Disable collision so you can move through objects
        intangibilityManager.Timer = dashDuration;
        intangibilityManager.flashType = flashType;

        int id = Random.Range(int.MinValue, int.MaxValue);
        // Apply dash velocity
        StartCoroutine(base.Dash());
        while (isDashing)
        {
            CheckDashHits(id);
            yield return null;
        }

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void CheckDashHits(int id)
    {
        // Detect objects the dash passes through (small sphere around player)
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, hitRadius, hitMask);

        foreach (Collider hit in hitObjects)
        {
            GameObject hitObject = hit.gameObject;
            
            // Ignore self
            if (hit.gameObject == gameObject) continue;

            // Try to find a PlayerStats component and deal damage
            AttackBase.Damage(hit.transform, dashDamage, id);

        }
    }
}
