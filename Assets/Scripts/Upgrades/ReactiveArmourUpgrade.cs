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

    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            PlayerHealth.OnPlayerDamaged += OnPlayerDamaged;
            Debug.Log("Reactive Armor Upgrade Activated!");
        }
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerDamaged -= OnPlayerDamaged;
    }

    private void OnPlayerDamaged()
    {
        if (!isActive || !canTrigger || player == null) return;
        StartCoroutine(TriggerReactivePulse());
    }

    private System.Collections.IEnumerator TriggerReactivePulse()
    {
        canTrigger = false;

        Collider[] hits = Physics.OverlapSphere(player.position, explosionRadius);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(explosionDamage);
                Rigidbody rb = enemy.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector2 knockbackDir = (enemy.transform.position - player.position).normalized;
                    rb.AddForce(knockbackDir * 200f);
                }
            }
        }

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
