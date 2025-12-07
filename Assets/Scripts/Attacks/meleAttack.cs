using UnityEngine;
using System.Collections;

public class meleAttack : AttackBase
{
    private bool attacking;
    private int id;
    protected override IEnumerator ExecuteAttack()
    {
        attacking = true;
        id = Random.Range(int.MinValue, int.MaxValue);
        yield return StartCoroutine(animation.attackingStance());
        yield return StartCoroutine(animation.Attack());
        attacking = false;
        yield break;
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject attacked = other.transform.root.gameObject;
        Health health = attacked.GetComponent<Health>();
        if (health != null && attacking)
            health.TakeDamage(damage, id);
    }
}
