using System.Collections;
using UnityEngine;

public class Weapon : AttackBase
{
    public virtual void Attack()
    {
        Debug.Log("pow");
    }

    protected override IEnumerator ExecuteAttack()
    {
        Attack();
        yield break;
    }
}
