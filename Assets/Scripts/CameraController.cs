using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraController : MonoBehaviour
{
    private CinemachineInputAxisController controller;
    private PlayerInput controles;
    private void Start()
    {
        controller = GetComponent<CinemachineInputAxisController>();
        controles = GetComponent<PlayerInput>();
        controles.ActivateInput();
    }
    private void FixedUpdate()
    {
        float movement = controles.actions["Camera"].ReadValue<float>();
        for 
    }
}
