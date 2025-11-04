using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Fly : MonoBehaviour
{
    [Header("Contoll references")]
    public PlayerInput input;
    public CharacterController controller;
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
            controller.Move(Vector3.up * force * Time.deltaTime);
            fuel -= Time.deltaTime;
            movementScript.gravity = 0;
        }
        else if (controller.isGrounded)
            fuel = duration;
        else
            movementScript.gravity = defultGravity;
    }
}
