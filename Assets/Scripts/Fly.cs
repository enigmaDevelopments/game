using UnityEngine;
using UnityEngine.InputSystem;

public class Fly : MonoBehaviour
{
    [Header("Contoll references")]
    public PlayerInput input;
    public ThirdPersonMovement movementScript;
    [Header("Settings")]
    public float duration;
    public float force;

    private InputAction jump;
    private float fuel;
    private float defultGravity;

    private void Start()
    {
        jump = input.actions?.FindAction("Jump", throwIfNotFound: true);
        defultGravity = movementScript.gravity;
    }
    private void Update()
    {
        if (0 < fuel && jump.IsPressed())
        {
            movementScript.playerVelocity.y += force * Time.deltaTime;
            fuel -= Time.deltaTime;
            movementScript.gravity = 0;
        }
        else if (movementScript.IsGrounded)
            fuel = duration;
        else
            movementScript.gravity = defultGravity;
    }
}
