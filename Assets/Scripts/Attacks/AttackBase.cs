using System.Collections;
using UnityEngine;

public abstract class AttackBase : MonoBehaviour
{
    public new WeponAnimation animation;
    [Header("Attack Settings")]
    public float attackCooldown = 0.5f;

    // ?? Add these
    [SerializeField] protected float baseDamage = 10f;   // set in Inspector
    protected float currentDamage;                       // runtime damage

    public float CurrentDamage => currentDamage;         // read-only from outside

    public bool canHold;

    protected bool isAttacking;
    protected bool canAttack = true;

    public bool IsAttacking => isAttacking;
    public float Cooldown => attackCooldown;
    public bool CanHold => canHold;

    public virtual void SetDamage(float newDamage)
    {
        currentDamage = newDamage;
    }

    protected virtual void Awake()
    {
        // Initialize current damage from baseDamage
        currentDamage = baseDamage;
    }

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

    public static void Damage(Transform hit, float damage, int id)
    {
        Health health = hit.transform.root.GetComponent<Health>();
        if (health != null)
            health.TakeDamage(damage, id);
    }
    public static void Damage(Transform hit, float damage)
    {
        Health health = hit.transform.root.GetComponent<Health>();
        if (health != null)
            health.TakeDamage(damage);
    }
}
