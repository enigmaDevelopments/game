using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform target;
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
        float movement = controles.actions["Camera"].ReadValue<float>();
        target.Rotate(0, movement * speed, 0);
    }
}
