using UnityEngine;

public class DashThroughDamage : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Damage Settings")]
    public int dashDamage = 10; // how much damage to deal per dash hit

    private Rigidbody rb;
    private CapsuleCollider col;
    private bool canDash = true;
    private bool isDashing = false;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && moveDirection != Vector3.zero)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private System.Collections.IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        // ?? Disable collision so you can move through objects
        col.enabled = false;

        // Apply dash velocity
        rb.linearVelocity = moveDirection * dashSpeed;

        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            // During dash, check for objects to damage
            CheckDashHits();
            yield return null;
        }

        // Stop dash
        rb.linearVelocity = Vector3.zero;
        col.enabled = true; // re-enable collision

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void CheckDashHits()
    {
        // Detect objects the dash passes through (small sphere around player)
        float hitRadius = 0.5f;
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, hitRadius);

        foreach (Collider hit in hitObjects)
        {
            // Ignore self
            if (hit.gameObject == gameObject) continue;

            // Try to find a health component and deal damage
            var health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(dashDamage);
            }
        }
    }
}
