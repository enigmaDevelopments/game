using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class AimingSystem : MonoBehaviour
{
    [Header("Aiming Settings")]
    public float aimSensitivity = 2f;           // Mouse sensitivity while aiming
    
    [Header("Weapon")]
    public List<Transform> weaponTransforms;           // The weapon to rotate
    public List<Transform> rotationTransforms;
    public List<Quaternion> rotationOffesets; 
    public float returnSpeed;
    public float raycastDistance = 1000f;
    
    [Header("Layers")]
    public LayerMask raycastLayers;

    [Header("Animations")]
    public WeponAnimation[] canceledAnimations;

    private PlayerInput playerInput;
    private bool isAiming = false;
    private Vector2 aimInput;
    private CinemachineOrbitalFollow follower;
    private Quaternion[] lastRotations;
    private bool returning = false;
    private CinemachineCamera normalCamera;
    private CinemachineCamera aimCamera;
    private Canvas crosshairCanvas;

    public bool IsAiming => isAiming;
    
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        CameraData cameras = FindFirstObjectByType<CameraData>();
        normalCamera = cameras.normalCamera;
        aimCamera = cameras.aimCamera;
        crosshairCanvas = cameras.crosshairCanvas;



        if (playerInput == null)
        {
            LogError("AimingSystem: PlayerInput component not found!");
            return;
        }
        
        // Check if Aim action exists
        if (playerInput.actions.FindAction("Aim") == null)
        {
            LogError("AimingSystem: 'Aim' action not found in input actions!");
            return;
        }
        
        // Set up input callbacks
        playerInput.actions["Aim"].started += OnAimStarted;
        playerInput.actions["Aim"].canceled += OnAimCanceled;
        
        Log("AimingSystem: Input actions set up successfully");

        // Make sure cameras are set up
        if (normalCamera == null)
            LogError("AimingSystem: normalCamera not assigned!");
        if (aimCamera == null)
            LogError("AimingSystem: aimCamera not assigned!");

        // Hide crosshair initially
        if (crosshairCanvas != null)
            crosshairCanvas.gameObject.SetActive(false);
        follower = aimCamera.GetComponent<CinemachineOrbitalFollow>();
    }
    
    private void OnAimStarted(InputAction.CallbackContext context)
    {
        isAiming = true;
        Log("Aim started!");
        EnterAimMode();
    }
    
    private void OnAimCanceled(InputAction.CallbackContext context)
    {
        isAiming = false;
        Log("Aim ended!");
        ExitAimMode();
    }
    
    private void EnterAimMode()
    {
        // Show crosshair
        if (crosshairCanvas != null)
            crosshairCanvas.gameObject.SetActive(true);
        
        // Disable normal camera and enable aim camera IMMEDIATELY
        if (normalCamera != null)
        {
            normalCamera.enabled = false;
            // Force priority low so it won't interfere
            normalCamera.Priority = 0;
        }
        
        if (aimCamera != null)
        {
            // Set aim camera to highest priority so it takes over immediately
            aimCamera.Priority = 100;
            aimCamera.enabled = true;
        }
        //Move camera behind player
        float rotation = normalCamera.transform.eulerAngles.y;
        Vector3 angles = transform.eulerAngles;
        angles.y = rotation;
        transform.eulerAngles = angles;
        follower.HorizontalAxis.Value = rotation;
        follower.VerticalAxis.Value = follower.VerticalAxis.Center;
        ThirdPersonMovement.aiming = true;
        //Stop animations
        foreach (WeponAnimation animation in canceledAnimations)
            animation.loadEnabled = false;
        lastRotations = new Quaternion[rotationTransforms.Count];
        for (int i = 0; i < rotationTransforms.Count; i++)
            lastRotations[i] = rotationTransforms[i].localRotation;

        Log("Entered aim mode - camera switched to aim camera");
    }

    private void ExitAimMode()
    {
        // Hide crosshair
        if (crosshairCanvas != null)
            crosshairCanvas.gameObject.SetActive(false);
        
        // Re-enable normal camera with high priority
        if (aimCamera != null)
        {
            aimCamera.Priority = 0;
            aimCamera.enabled = false;
        }
        
        if (normalCamera != null)
        {
            normalCamera.Priority = 10;  // Higher than aim camera when disabled
            normalCamera.enabled = true;
        }
        ThirdPersonMovement.aiming = false;
        foreach (WeponAnimation animation in canceledAnimations)
            animation.loadEnabled = true;
        StartCoroutine(ReturnWepon());
        Log("Exited aim mode - camera switched back to normal");
    }

    private void Update()
    {
        if (!isAiming) return;
        
        // Get mouse input for aiming - use "Look" action (Vector2)
        aimInput = playerInput.actions["Look"].ReadValue<Vector2>();
        
        // Rotate the player based on mouse movement
        RotatePlayerWithMouse();
        
        // Update weapon rotation to face crosshair target
        UpdateWeaponRotation();
    }
    
    private void RotatePlayerWithMouse()
    {
        if (aimInput == Vector2.zero) return;
        
        // Rotate player based on horizontal mouse movement
        float horizontalRotation = aimInput.x * aimSensitivity * Time.deltaTime;
        transform.Rotate(0, horizontalRotation, 0);
        follower.HorizontalAxis.Value += horizontalRotation;
    }
    private void UpdateWeaponRotation()
    {
        if (weaponTransforms.Count == 0) return;

        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        // Cast a ray from center of screen
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        Vector3 targetPoint;

        // Check if ray hits something
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, raycastLayers))
        {
            targetPoint = hit.point;
        }
        else
        {
            // If nothing hit, use a point far along the ray
            targetPoint = ray.origin + ray.direction * raycastDistance;
        }

        // Rotate weapon to face the target point
        for (int i = 0; i < rotationTransforms.Count; i++)
        {
            Vector3 directionToTarget = (targetPoint - weaponTransforms[i].position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget) * Quaternion.Inverse(rotationOffesets[i]);

            // Smoothly rotate weapon
            rotationTransforms[i].rotation = Quaternion.Slerp(
                rotationTransforms[i].rotation,
                targetRotation,
                Time.deltaTime * 10f
            );
        }
    }

    private IEnumerator ReturnWepon()
    {
        if (returning) 
            yield break;
        returning = true;
        bool finished;
        do
        {
            finished = true;
            for (int i = 0; i < rotationTransforms.Count; i++)
                if (rotationTransforms[i].localRotation != lastRotations[i])
                {
                    finished = false;
                    rotationTransforms[i].localRotation = Quaternion.RotateTowards(rotationTransforms[i].localRotation, lastRotations[i], returnSpeed * Time.deltaTime);
                }
            yield return null;
        } while (!finished);
        returning = false;
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.actions["Aim"].started -= OnAimStarted;
            playerInput.actions["Aim"].canceled -= OnAimCanceled;
        }
    }
    private static void Log(string message)
    {
        #if UNITY_EDITOR
            Debug.Log(message);
        #endif
    }
    private static void LogError(string message)
    {
        #if UNITY_EDITOR
            Debug.LogError(message);
        #endif
    }
}


