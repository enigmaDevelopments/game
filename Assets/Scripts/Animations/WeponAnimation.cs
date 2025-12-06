using System.Collections;
using UnityEngine;

public abstract class WeponAnimation : MonoBehaviour
{
    [Header("Timings")]
    public float loadSeconds;
    public float attackSeconds;
    protected float loadTimer = float.Epsilon;
    protected float attackTimer = float.Epsilon;
    private bool changingStance = false;
    private bool attacking = false;
    private bool stanceReverse = false;
    private bool attackReverse = false;
    public virtual IEnumerator attackingStance(bool reverse = false)
    {
        stanceReverse = reverse;
        if (changingStance)
        {
            yield return new WaitUntil(() => changingStance);
            yield break;
        }
        changingStance = true;
        for (; loadTimer < 1 && 0 < loadTimer; loadTimer += Time.deltaTime / loadSeconds * (stanceReverse ? -1 : 1))
        {
            moveStance();
            yield return null;
        }
        loadTimer = Mathf.Clamp(loadTimer,float.Epsilon,1-float.Epsilon);
        changingStance = false;
        yield break;

    }
    public virtual IEnumerator Attack(bool reverse = false)
    {
        attackReverse = reverse;
        if (attacking)
        {
            yield return new WaitUntil(() => attacking);
            yield break;
        }
        attacking = true;
        for (; 0 < attackTimer; attackTimer += Time.deltaTime / attackSeconds * (attackReverse ? -1 : 1))
        {
            moveAttack();
            yield return null;
            if (1 <= attackTimer)
                attackReverse = true;
        }
        attackTimer = float.Epsilon;
        attacking = false;
        yield break;
    }
    protected abstract void moveStance();
    protected abstract void moveAttack();

}
