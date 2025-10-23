using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    [Header("Input")]
    [Tooltip("Optional. If not provided, will try to GetComponent<PlayerInput>()")] 
    public PlayerInput input;

    [Header("Attacks")]
    public AttackBase primaryAttack;   // Melee
    public AttackBase secondaryAttack; // Projectile

    private void Awake()
    {
        if (input == null)
            input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (input != null)
        {
            var actions = input.actions;
            var melee = actions?.FindAction("MeleeAttack", throwIfNotFound: false);
            var projectile = actions?.FindAction("ProjectileAttack", throwIfNotFound: false);

            if (melee != null && melee.WasPerformedThisFrame())
            {
                TryPrimary();
            }

            if (projectile != null && projectile.WasPerformedThisFrame())
            {
                TrySecondary();
            }
        }
    }

    // Public methods so AI can trigger attacks without input
    public bool TryPrimary()
    {
        Debug.Log("Trying Primary Attack");
        return primaryAttack != null && primaryAttack.TryAttack();
    }

    public bool TrySecondary()
    {
        Debug.Log("Trying Secondary Attack");
        return secondaryAttack != null && secondaryAttack.TryAttack();
    }
}
