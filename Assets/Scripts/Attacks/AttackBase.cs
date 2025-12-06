using System.Collections;
using UnityEngine;

public abstract class AttackBase : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackCooldown = 0.5f;
    public float damage;
    public bool canHold;

    protected bool isAttacking;
    protected bool canAttack = true;

    public bool IsAttacking => isAttacking;
    public float Cooldown => attackCooldown;
    public bool CanHold => canHold;

    // Public entry point that consumers (like AttackController) call
    public bool TryAttack()
    {
        if (!canAttack || isAttacking)
            return false;

        StartCoroutine(AttackFlow());
        return true;
    }

    private IEnumerator AttackFlow()
    {
        canAttack = false;
        isAttacking = true;

        // Perform the concrete attack behavior (may yield for duration/animation)
        yield return ExecuteAttack();

        isAttacking = false;

        // Enforce cooldown after the attack completes
        if (attackCooldown > 0f)
        {
            yield return new WaitForSeconds(attackCooldown);
        }

        canAttack = true;
    }

    // Implement per-attack behavior. Return an IEnumerator to support animations/timings.
    protected abstract IEnumerator ExecuteAttack();
}
