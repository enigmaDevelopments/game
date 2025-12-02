using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public new Transform camera;
    public float speed = 1.0f;
    public float aimSpeed = 0.5f;  // Reduced speed while aiming
    
    private PlayerInput controles;
    private Rigidbody rb;
    private AimingSystem aimingSystem;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controles = GetComponent<PlayerInput>();
        aimingSystem = GetComponent<AimingSystem>();
        controles.ActivateInput();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 movement = controles.actions["Move"].ReadValue<Vector2>();
        movement = Vector2.ClampMagnitude(movement, 1);
        
        // Use reduced speed while aiming
        float currentSpeed = (aimingSystem != null && aimingSystem.IsAiming) ? aimSpeed : speed;
        movement = movement * currentSpeed;
        
        Vector3 movement3d = new Vector3(movement.x, 0.0f, movement.y);
        
        // If aiming, move relative to player's current forward direction
        if (aimingSystem != null && aimingSystem.IsAiming)
        {
            movement3d = transform.TransformDirection(movement3d);
        }
        else
        {
            // Normal mode: move relative to camera direction
            Vector3 cameraDirection = (transform.position - camera.position);
            cameraDirection.y = 0;
            cameraDirection.Normalize();
            movement3d = Quaternion.LookRotation(cameraDirection) * movement3d;
        }

        rb.linearVelocity = movement3d;
        
        // Don't rotate player if aiming (mouse controls rotation)
        if (movement != Vector2.zero && (aimingSystem == null || !aimingSystem.IsAiming))
            rb.MoveRotation(Quaternion.LookRotation(movement3d));
    }
}
