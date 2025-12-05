using UnityEngine.SceneManagement;
public class PlayerStats : Health
{
    public float intangibilityDuration;
    public HealthBar healthBar;
    public int deathScene;
    private IntangibilityManager intangibilityManager;

    protected override void Start()
    {
        base.Start();
        intangibilityManager = GetComponent<IntangibilityManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.SetHealth((int)health);
        canTakeDamage = false;
        intangibilityManager.Timer = intangibilityDuration;
    }
    protected override void Die()
    {
        SceneManager.LoadScene(deathScene);
    }
}