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

    public InputAction dash;
    private bool canDash = true;

    

    private void Start()
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

    private System.Collections.IEnumerator Dash()
    {
        canDash = false;

        float startTime = Time.time;
        Vector3 direction = transform.forward;
        while (Time.time < startTime + dashDuration)
        {
            controller.Move(direction * dashSpeed * Time.deltaTime) ;
            yield return null;
        }

        // Wait before you can dash again
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

}
