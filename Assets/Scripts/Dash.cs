using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    public CharacterController controller;
    public PlayerInput input;
    
    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    protected InputAction dash;
    protected bool canDash = true;
    protected bool isDashing = false;
    

    protected virtual void Start()
    {
        dash = input.actions?.FindAction("Dash", throwIfNotFound: false);
    }
    void Update()
    {
        // Dash input (press Left Shift)
        if (dash.WasPressedThisFrame() && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    protected virtual System.Collections.IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        Vector3 direction = transform.forward;
        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            controller.Move(direction * dashSpeed * Time.deltaTime) ;
            yield return null;
        }

        isDashing = false;
        // Wait before you can dash again
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

}
