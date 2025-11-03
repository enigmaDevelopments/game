using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private bool isDashing = false;
    private bool canDash = true;
    private Vector3 moveInput;

    void Update()
    {
        // Get basic input

        // Move normally when not dashing
        if (!isDashing)
        {
            transform.Translate(moveInput * moveSpeed * Time.deltaTime, Space.World);
        }

        // Dash input (press Left Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && moveInput != Vector3.zero)
        {
            StartCoroutine(Dash());
        }
    }

    private System.Collections.IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;

        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            transform.Translate(moveInput * dashSpeed * Time.deltaTime, Space.World);
            yield return null;
        }

        isDashing = false;

        // Wait before you can dash again
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
