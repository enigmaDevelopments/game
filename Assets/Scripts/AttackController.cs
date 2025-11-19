using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    [Header("Scripts")]
    [Tooltip("Optional. If not provided, will try to GetComponent<PlayerInput>()")] 
    public PlayerInput input;

    [Tooltip("Optional. If not provided, will try to GetComponent<CharacterBrain>()")]
    public CharacterBrain brain;

    [Header("Attacks")]
    public AttackBase primaryAttack;   // Melee
    public AttackBase secondaryAttack; // Projectile
    public AttackBase tertiaryAttack; // special

    private void Awake()
    {
        if (input == null)
            input = GetComponent<PlayerInput>();
        if (brain == null)
            brain = GetComponent<CharacterBrain>();
    }

    private void Update()
    {
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
}
