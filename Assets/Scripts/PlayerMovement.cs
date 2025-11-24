using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float baseSpeed = 5f;                // Normal player speed
    [HideInInspector] public float momentumMultiplier = 1f; // <— Needed for MomentumUpgrade

    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    private EvasiveMomentumUpgrade evasiveMomentumUpgrade;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        evasiveMomentumUpgrade = FindFirstObjectByType<EvasiveMomentumUpgrade>();
    }

    void Update()
    {
        // Movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Example dodge input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformDodge();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void PerformDodge()
    {
        // Placeholder dodge logic (add animation or physics dash here)
        Debug.Log("Player dodged!");

        // Notify the upgrade system
        evasiveMomentumUpgrade?.OnDodge();
    }

    public Vector2 GetMovementDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        return new Vector2(horizontal, vertical).normalized;
    }
}
