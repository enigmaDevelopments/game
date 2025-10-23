using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public new Transform camera;
    public float speed = 1.0f;
    private PlayerInput controles;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controles = GetComponent<PlayerInput>();
        controles.ActivateInput();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 movement = controles.actions["Move"].ReadValue<Vector2>();
        movement = Vector2.ClampMagnitude(movement, 1) * speed;
        Vector3 movement3d = new Vector3(movement.x, 0.0f, movement.y);
        // Adjust movement direction based on camera orientation
        Vector3 cameraDirection = (transform.position - camera.position);
        cameraDirection.y = 0;
        cameraDirection.Normalize();
        movement3d = Quaternion.LookRotation(cameraDirection) * movement3d;

        rb.linearVelocity = movement3d;
        if (movement != Vector2.zero)
            rb.MoveRotation(Quaternion.LookRotation(movement3d));
    }
}
