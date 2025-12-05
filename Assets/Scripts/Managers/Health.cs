using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    public float health;
    protected virtual void Start()
    {
        health = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    protected virtual void Die() { }
}
