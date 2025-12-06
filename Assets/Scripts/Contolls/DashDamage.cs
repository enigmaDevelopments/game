using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DashThroughDamage : PlayerDash
{
    [Header("Damage Settings")]
    public int dashDamage = 10; // how much damage to deal per dash hit
    public float hitRadius = 0.5f;
    [Header("Layer Settings")]
    public LayerMask hitMask;
    private IntangibilityManager intangibilityManager;
    private List<GameObject> hits = new();

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

        // Apply dash velocity
        StartCoroutine(base.Dash());
        while (isDashing)
        {
            CheckDashHits();
            yield return null;
        }
        hits.Clear();

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
            GameObject hitObject = hit.gameObject;
            
            // Ignore self
            if (hit.gameObject == gameObject) continue;
            if (hits.Contains(hitObject)) continue;
            hits.Add(hitObject);

            // Try to find a PlayerStats component and deal damage
            var health = hit.GetComponent<Health>();
            if (health != null)
                health.TakeDamage(dashDamage);

        }
    }
}
