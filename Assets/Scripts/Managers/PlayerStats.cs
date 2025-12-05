using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerStats : Health
{
    public HealthBar healthBar;
    public int deathScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.SetHealth((int)health);
    }
    protected override void Die()
    {
        SceneManager.LoadScene(deathScene);
    }
}