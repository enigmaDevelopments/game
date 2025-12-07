using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Health : MonoBehaviour
{
    public float maxHealth = 7;
    [HideInInspector]  public bool canTakeDamage = true;
    protected float health;
    private List<int> attackIds = new();
    protected virtual void Start()
    {
        health = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        if (!canTakeDamage) return;
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    public virtual void TakeDamage(float damage, int attackId)
    {
        if (!canTakeDamage) return;
        if (attackIds.Contains(attackId)) return;
        attackIds.Add(attackId);
        removeId(attackId);
        TakeDamage(damage);
    }
    private IEnumerator removeId(int id) {         
        yield return new WaitForSeconds(100f);
        attackIds.Remove(id);
    }

    protected abstract void Die();
}
