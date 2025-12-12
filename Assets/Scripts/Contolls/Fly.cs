using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fly : ThirdPersonMovement
{

    [Header("Flight Settings")]
    public float duration;
    public float force;

    private float fuel;
    private bool flying;

    protected override void Start()
    {
        canFastFall = false;
        base.Start();
    }
    public override void OnJump(InputValue value)
    {
        flying = value.isPressed;
        base.OnJump(value);
        
    }

    protected override void Update()
    {
        if (0 < fuel && flying)
        {
            playerVelocity.y += force * Time.deltaTime;
            fuel -= Time.deltaTime;
        }
        else if (isGrounded)
            fuel = duration;
        base.Update();
    }
}
