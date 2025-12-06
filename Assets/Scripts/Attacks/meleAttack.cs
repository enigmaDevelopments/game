using UnityEngine;
using System.Collections;

public class meleAttack : AttackBase
{
    protected override IEnumerator ExecuteAttack()
    {
        yield return StartCoroutine(animation.attackingStance());
        yield return StartCoroutine(animation.Attack());
        yield break;
    }
}
