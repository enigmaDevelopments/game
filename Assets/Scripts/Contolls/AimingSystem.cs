using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimingSystem : MonoBehaviour
{
    [Header("Camera Setup")]
    public CinemachineCamera normalCamera;
    public CinemachineCamera aimCamera;
    
    [Header("Aiming Settings")]
    public float aimSensitivity = 2f;           // Mouse sensitivity while aiming
    
    [Header("Crosshair")]
    public Canvas crosshairCanvas;
    public RectTransform crosshairImage;
    
    [Header("Weapon")]
    public Transform weaponTransform;           // The weapon to rotate
    public float raycastDistance = 1000f;
    
    [Header("Layers")]
    public LayerMask raycastLayers;
    
    private PlayerInput playerInput;
    private bool isAiming = false;
    private Vector2 aimInput;
    private CinemachineOrbitalFollow follower;

    public bool IsAiming => isAiming;
    
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        
        if (playerInput == null)
        {
            Debug.LogError("AimingSystem: PlayerInput component not found!");
            return;
        }
        
        // Check if Aim action exists
        if (playerInput.actions.FindAction("Aim") == null)
        {
            Debug.LogError("AimingSystem: 'Aim' action not found in input actions!");
            return;
        }
        
        // Set up input callbacks
        playerInput.actions["Aim"].started += OnAimStarted;
        playerInput.actions["Aim"].canceled += OnAimCanceled;
        
        Debug.Log("AimingSystem: Input actions set up successfully");
        
        // Make sure cameras are set up
        if (normalCamera == null)
            Debug.LogError("AimingSystem: normalCamera not assigned!");
        if (aimCamera == null)
            Debug.LogError("AimingSystem: aimCamera not assigned!");
        
        // Hide crosshair initially
        if (crosshairCanvas != null)
            crosshairCanvas.gameObject.SetActive(false);
        follower = aimCamera.GetComponent<CinemachineOrbitalFollow>();
    }
    
    private void OnAimStarted(InputAction.CallbackContext context)
    {
        isAiming = true;
        Debug.Log("Aim started!");
        EnterAimMode();
    }
    
    private void OnAimCanceled(InputAction.CallbackContext context)
    {
        isAiming = false;
        Debug.Log("Aim ended!");
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
        //move camera behind player
        follower.HorizontalAxis.Value = transform.eulerAngles.y;
        ThirdPersonMovement.aiming = true;
        #if UNITY_EDITOR
            Debug.Log("Entered aim mode - camera switched to aim camera");
        #endif
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
        #if UNITY_EDITOR
            Debug.Log("Exited aim mode - camera switched back to normal");
        #endif
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
        if (weaponTransform == null) return;
        
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
        Vector3 directionToTarget = (targetPoint - weaponTransform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        
        // Smoothly rotate weapon
        weaponTransform.rotation = Quaternion.Slerp(
            weaponTransform.rotation,
            targetRotation,
            Time.deltaTime * 10f
        );
    }
    
    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.actions["Aim"].started -= OnAimStarted;
            playerInput.actions["Aim"].canceled -= OnAimCanceled;
        }
    }
}


