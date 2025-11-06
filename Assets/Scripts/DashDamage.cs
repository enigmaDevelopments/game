using UnityEngine;

public class DashThroughDamage : PlayerDash
{
    [Header("Damage Settings")]
    public int dashDamage = 10; // how much damage to deal per dash hit
    public float hitRadius = 0.5f;
    public LayerMask hitMask;

    private Collider col;

    protected override void Start()
    {
        col = GetComponent<Collider>();
        base.Start();
    }

    protected override System.Collections.IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        // ?? Disable collision so you can move through objects
        col.enabled = false;

        // Apply dash velocity
        StartCoroutine(base.Dash());
        while (isDashing)
        {
            CheckDashHits();
            yield return null;
        }

        col.enabled = true; // re-enable collision

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void CheckDashHits()
    {
        // Detect objects the dash passes through (small sphere around player)
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, hitRadius, hitMask);

        foreach (Collider hit in hitObjects)
        {
            // Ignore self
            if (hit.gameObject == gameObject) continue;

            // Try to find a PlayerStats component and deal damage
            var playerStats = hit.GetComponent<PlayerStats>();
            playerStats.TakeDamage(dashDamage);

        }
    }
}
