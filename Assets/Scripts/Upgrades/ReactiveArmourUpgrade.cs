using UnityEngine;

public class ReactiveArmorUpgrade : MonoBehaviour
{
    [Header("Reactive Armor Settings")]
    [Tooltip("Enable or disable the upgrade")]
    public bool isActive = false;

    [Tooltip("Radius of the explosion effect")]
    public float explosionRadius = 3f;

    [Tooltip("Damage dealt to enemies within radius")]
    public float explosionDamage = 20f;

    [Tooltip("Cooldown between explosions")]
    public float cooldown = 3f;

    private bool canTrigger = true;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        PlayerHealth.OnPlayerDamaged += OnPlayerDamaged;
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerDamaged -= OnPlayerDamaged;
    }

    public void Activate()
    {
        isActive = true;
        Debug.Log("Reactive Armor Upgrade Activated!");
    }

    private void OnPlayerDamaged()
    {
        if (!isActive || !canTrigger || player == null) return;
        StartCoroutine(TriggerReactivePulse());
    }

    private System.Collections.IEnumerator TriggerReactivePulse()
    {
        canTrigger = false;

        Debug.Log("?? Reactive Armor triggered!");

        // Detect enemies within radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(player.position, explosionRadius);

        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(explosionDamage);
                // Optional: Apply knockback
                Vector2 knockbackDir = (enemy.transform.position - player.position).normalized;
                Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.AddForce(knockbackDir * 200f); // tweak force as needed
            }
        }

        // Optional: Add visual or sound effect here
        // e.g. Instantiate(explosionEffectPrefab, player.position, Quaternion.identity);

        yield return new WaitForSeconds(cooldown);
        canTrigger = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (player == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, explosionRadius);
    }
}
