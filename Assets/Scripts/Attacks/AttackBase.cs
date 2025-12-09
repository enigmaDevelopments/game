using System.Collections;
using UnityEngine;

public abstract class AttackBase : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] protected float attackCooldown = 0.5f;
    [SerializeField] private bool canHold;
    [SerializeField] protected float baseDamage = 10f;

    protected bool isAttacking;
    protected bool canAttack = true;

    public bool IsAttacking => isAttacking;
    public float Cooldown => attackCooldown;
    public bool CanHold => canHold;
    public float Damage => baseDamage;

    public bool TryAttack()
    {
        if (!canAttack || isAttacking)
            return false;

        StartCoroutine(AttackFlow());
        return true;
    }

    public void SetDamage(float newDamage)
    {
        baseDamage = newDamage;
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
