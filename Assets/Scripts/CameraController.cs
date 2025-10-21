using Unity.VisualScripting;
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
        target.position = transform.position;
    }
}
