using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    [Header("Control Mode")]
    [Tooltip("If true, attacks are controlled by AI instead of player input")]
    public bool isAIControlled = false;

    [Header("Scripts")]
    [Tooltip("Optional. If not provided, will try to GetComponent<PlayerInput>()")] 
    public PlayerInput input;

    [Tooltip("Optional. If not provided, will try to GetComponent<CharacterBrain>()")]
    public CharacterBrain brain;

    [Header("Attacks")]
    public AttackBase primaryAttack;   // Melee
    public AttackBase secondaryAttack; // Projectile
    public AttackBase tertiaryAttack; // special
    [Header ("Animations")]
    public WeponAnimation primaryAnimation;   // Melee
    public WeponAnimation secondaryAnimation; // Projectile
     public WeponAnimation tertiaryAnimation; // special

    [Header("AI Settings")]
    [Tooltip("Reference to player/target for AI attacks")]
    public Transform aiTarget;

    [Tooltip("Tag to find AI target if not manually set")]
    public string aiTargetTag = "Player";

    private void Awake()
    {
        if (input == null)
            input = GetComponent<PlayerInput>();
        if (brain == null)
            brain = GetComponent<CharacterBrain>();
    }


    private void Start()
    {
        // Find AI target if in AI mode and no target is set
        if (isAIControlled && aiTarget == null)
        {
            GameObject targetObj = GameObject.FindGameObjectWithTag(aiTargetTag);
            if (targetObj != null)
            {
                aiTarget = targetObj.transform;
            }
        }

        if (primaryAttack != null && primaryAnimation != null)
            primaryAttack.animation = primaryAnimation;
        if (secondaryAttack != null && secondaryAnimation != null)
            secondaryAttack.animation = secondaryAnimation;
        if (tertiaryAttack != null && tertiaryAnimation != null)
            tertiaryAttack.animation = tertiaryAnimation;
    }

    private void Update()
    {
        if (isAIControlled)
        {
            // AI mode - let individual attack AI behaviors handle when to attack
            // The attacks will call TryPrimary/TrySecondary/TryTertiary themselves
            return;
        }

        // Player input mode
        if (input != null)
        {
            var actions = input.actions;
            var melee = actions?.FindAction("MeleeAttack", throwIfNotFound: false);
            var projectile = actions?.FindAction("ProjectileAttack", throwIfNotFound: false);
            var special = actions?.FindAction("Special", throwIfNotFound: false);

            if (brain.hasMeleeWeapon && ((primaryAttack.CanHold && melee.IsPressed()) || melee.WasPerformedThisFrame()))
            {
                TryPrimary();
            }

            if (brain.hasProjectileWeapon && ((secondaryAttack.CanHold && projectile.IsPressed()) || projectile.WasPerformedThisFrame()))
            {
                TrySecondary();
            }
            if (brain.hasSpecialeWeapon && ((tertiaryAttack.CanHold && special.IsPressed()) || special.WasPerformedThisFrame()))
            {
                TryTertiary();
            }
        }
    }

    // Public methods so AI can trigger attacks without input
    public bool TryPrimary()
    {
        return TryAttack(primaryAttack);
    }

    public bool TrySecondary()
    {
        return TryAttack(secondaryAttack);
    }
    
    public bool TryTertiary()
    {
        return TryAttack(tertiaryAttack);
    }

    private static bool TryAttack(AttackBase attack)
    {
        return attack != null && attack.TryAttack();
    }

    // Helper methods for AI to check attack readiness
    public bool CanUsePrimary()
    {
        return primaryAttack != null && !primaryAttack.IsAttacking && brain.hasMeleeWeapon;
    }

    public bool CanUseSecondary()
    {
        return secondaryAttack != null && !secondaryAttack.IsAttacking && brain.hasProjectileWeapon;
    }

    public bool CanUseTertiary()
    {
        return tertiaryAttack != null && !tertiaryAttack.IsAttacking && brain.hasSpecialeWeapon;
    }
}
