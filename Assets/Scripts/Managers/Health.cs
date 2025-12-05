using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 7;
    public bool canTakeDamage = true;
    protected float health;
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
    protected virtual void Die() { }
}
