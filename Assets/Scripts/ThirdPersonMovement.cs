using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    
    [Header("Movement Settings")]
    public float maxSpeed = 8f;
    public float acceleration = 60f;
    public float deceleration = 40f;
    public float turnSpeed = 10f;
    
    [Header("Jump Settings")]
    public float jumpForce = 8f;
    public float gravity = -20f;

    [Header("Timer")]
    public float bufferTime = .5f;

    protected Vector3 playerVelocity;
    private Vector2 movement;
    private Vector3 currentMoveDirection;
    private Vector3 lastMoveDirection;
    private float currentSpeed;
    private float coyoteTimeCounter;
    protected bool isGrounded;
    private bool wasGrounded;
    private float buffer = 0;
    private Transform cameraTransform;
    protected bool canFasttFall = true;

    public void OnMove(InputValue value)
    {
        movement = Vector2.ClampMagnitude(value.Get<Vector2>(),1);
    }

    protected virtual void Start()
    {
        cameraTransform = Camera.main.transform;

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public virtual void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (isGrounded)
                playerVelocity.y = jumpForce;
            else
                buffer = bufferTime;
        }
        else if (canFasttFall && 0 < playerVelocity.y)
            playerVelocity.y = 0;
        else
            buffer = 0;
    }

    protected virtual void Update()
    {
        isGrounded = controller.isGrounded;
        // Apply gravity
        if (!isGrounded)
        {
            playerVelocity.y += gravity * Time.deltaTime;

            //decrment buffertime timer
            if (0 < buffer)
                buffer -= Time.deltaTime;
        }
        else if (playerVelocity.y < 0)
        {
            // Reset vertical velocity when grounded
            playerVelocity.y = -2f;

            //Jump if jump is buffered
            if (0 < buffer)
                playerVelocity.y = jumpForce;
        }

        // Get camera-relative movement direction
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 targetDirection = (forward * movement.y + right * movement.x).normalized;

        // Handle movement and acceleration
        if (targetDirection.magnitude >= 0.1f)
        {
            // Accelerate
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
            currentMoveDirection = Vector3.Lerp(currentMoveDirection, targetDirection, turnSpeed * Time.deltaTime);
            lastMoveDirection = currentMoveDirection;
        }
        else
        {
            // Decelerate
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
            if (currentSpeed < 0.1f)
            {
                currentSpeed = 0;
            }
        }

        // Rotate player
        if (currentSpeed > 0.1f)
        {
            float targetAngle = Mathf.Atan2(currentMoveDirection.x, currentMoveDirection.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        else if (lastMoveDirection != Vector3.zero)
        {
            // Keep facing the last movement direction when stopped
            float lastAngle = Mathf.Atan2(lastMoveDirection.x, lastMoveDirection.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, lastAngle, 0);
        }

        // Apply movement
        Vector3 moveVector = currentMoveDirection * currentSpeed;
        controller.Move(moveVector * Time.deltaTime);
        
        // Apply gravity and vertical movement
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
