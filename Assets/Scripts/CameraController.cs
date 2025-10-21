using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Rigidbody target;
    public float speed = 1f;
    private PlayerInput controles;

    void Start()
    {
        controles = GetComponent<PlayerInput>();
        controles.ActivateInput();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float movement = controles.actions["Camera"].ReadValue<float>() * speed;
        target.MoveRotation(Quaternion.Euler(0, movement + target.rotation.eulerAngles.y, 0));
        target.MovePosition(transform.position);
    }
}
