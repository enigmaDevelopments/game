using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.0f;
    private PlayerInput controles;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controles = GetComponent<PlayerInput>();
        controles.ActivateInput();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 movement = controles.actions["Move"].ReadValue<Vector2>();
        movement = Vector2.ClampMagnitude(movement, 1) * speed;
        Vector3 movement3d = new Vector3(movement.x, 0.0f, movement.y);
        rb.linearVelocity = movement3d;
        if (movement != Vector2.zero)
            rb.MoveRotation(Quaternion.LookRotation(movement3d));
    }
}
