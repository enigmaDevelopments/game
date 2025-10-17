using UnityEngine;
using UnityEngine.InputSystem;

public class FireProjectile : MonoBehaviour
{
    
    public Rigidbody projectile;
    // Speed of the projectile when fired.
    // This is a public variable so it can be adjusted in the Unity Editor.
    public float speed = 4;
    // Update is called once per frame
    // This method checks for input and fires a projectile if the attack action is pressed.
    void Update()
    {
        // Check if the "Attack" action was pressed this frame
        // If it was, instantiate a projectile at the player's position and set its velocity.
        if (InputSystem.actions.FindAction("Attack").WasPressedThisFrame())
        {
            Rigidbody p = Instantiate(projectile, transform.position, transform.rotation);
            p.linearVelocity = transform.forward * speed;
        }
    }
}


