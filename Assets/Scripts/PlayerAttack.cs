using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : AttackBase
{
    [Header("Melee Attack Settings")]
    [SerializeField] private float attackDuration = 0.15f;
    [SerializeField] private float rotationAngle = 45f;

    [Header("Hit Detection")]
    [SerializeField] private float hitRange = 0.8f;     // distance in front of player to check
    [SerializeField] private float hitRadius = 0.6f;    // radius of the sweep sphere
    [SerializeField] private LayerMask hitMask = ~0;    // what layers can be hit
    [SerializeField] private bool debugHitbox = false;  // draw gizmos for the hit area

    [Header("Slash Effect")]
    public GameObject slashEffectPrefab;
    private GameObject activeSlash;

    private Quaternion startRotation;
    private Quaternion attackRotation;

    // Reuse buffers to avoid GC allocs during the swing
    private readonly Collider[] hitBuffer = new Collider[32];

    // Input is handled by AttackController; this class only knows how to execute the attack
    protected override IEnumerator ExecuteAttack()
    {
        Console.WriteLine("SWINGING");

        // Spawn slash effect
        if (slashEffectPrefab != null)
        {
            activeSlash = Instantiate(
                slashEffectPrefab,
                transform.position + transform.forward * 0.7f,
                transform.rotation,
                transform   // parent to player so it moves with them
            );

            activeSlash.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Destroy(activeSlash, 1.0f);
        }

        // Simple rotation swing animation
        startRotation = transform.rotation;
        attackRotation = transform.rotation * Quaternion.Euler(0, rotationAngle, 0);

        var alreadyHit = new HashSet<Transform>();

        float elapsed = 0f;
        while (elapsed < attackDuration)
        {
            elapsed += Time.deltaTime * 4f;
            transform.rotation = Quaternion.Slerp(startRotation, attackRotation, elapsed);

            // During the active frames of the swing, check for hits
            DoMeleeHitCheck(alreadyHit);

            yield return null;
        }

        // Final check at the end of the swing
        DoMeleeHitCheck(alreadyHit);

        // Reset rotation after swing
        transform.rotation = startRotation;

        // Attack completes this frame; cooldown is handled by base class
        yield break;
    }

    private void DoMeleeHitCheck(HashSet<Transform> alreadyHit)
    {
        Vector3 center = transform.position + transform.forward * hitRange;

        if (debugHitbox)
        {
            Debug.DrawLine(transform.position, center, Color.yellow, 0.05f);
            DebugDrawWireSphere(center, hitRadius, Color.red, 0.05f);
        }

        int count = Physics.OverlapSphereNonAlloc(center, hitRadius, hitBuffer, hitMask, QueryTriggerInteraction.Ignore);
        Transform myRoot = transform.root;

        for (int i = 0; i < count; i++)
        {
            Collider c = hitBuffer[i];
            if (c == null) continue;

            Transform targetRoot = c.transform.root;

            // Ignore self or own children (including slash effect)
            if (targetRoot == myRoot)
                continue;

            // Avoid multi-hitting same target within one swing
            if (!alreadyHit.Add(targetRoot))
                continue;

            // Try to apply damage if a compatible component exists, otherwise just log
            // var health = targetRoot.GetComponentInChildren<Health>();
            // if (health != null) health.TakeDamage(damageAmount);

            Debug.Log($"Melee hit {targetRoot.name}");
        }

        // Clear buffer refs for safety (not necessary but helps when inspecting in debugger)
        for (int i = 0; i < count; i++) hitBuffer[i] = null;
    }

    private void DebugDrawWireSphere(Vector3 center, float radius, Color color, float duration)
    {
        // Minimal wire sphere drawing using circles on each axis
        const int segments = 20;
        Vector3 prev = Vector3.zero;
        Vector3 cur = Vector3.zero;

        // XY
        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments * Mathf.PI * 2f;
            cur = center + new Vector3(Mathf.Cos(t) * radius, Mathf.Sin(t) * radius, 0f);
            if (i > 0) Debug.DrawLine(prev, cur, color, duration);
            prev = cur;
        }
        // XZ
        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments * Mathf.PI * 2f;
            cur = center + new Vector3(Mathf.Cos(t) * radius, 0f, Mathf.Sin(t) * radius);
            if (i > 0) Debug.DrawLine(prev, cur, color, duration);
            prev = cur;
        }
        // YZ
        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments * Mathf.PI * 2f;
            cur = center + new Vector3(0f, Mathf.Cos(t) * radius, Mathf.Sin(t) * radius);
            if (i > 0) Debug.DrawLine(prev, cur, color, duration);
            prev = cur;
        }
    }
}
