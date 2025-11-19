using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float baseDamage = 10f;
    [HideInInspector] public float damageMultiplier = 1f;

    public void DealDamage(Enemy enemy)
    {
        float finalDamage = baseDamage * damageMultiplier;
        enemy.TakeDamage(finalDamage);
        Debug.Log($"Dealt {finalDamage} damage (Momentum x{damageMultiplier:F2})");
    }
}
