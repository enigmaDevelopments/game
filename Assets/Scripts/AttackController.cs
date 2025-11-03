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

    [Header("Press and Hold")]
    public bool canHoldPrimary;
    public bool canHoldSecondary;

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
            var melee = actions?.FindAction("MeleeAttack", throwIfNotFound: true);
            var projectile = actions?.FindAction("ProjectileAttack", throwIfNotFound: true);

            if (brain.hasMeleeWeapon && ((canHoldPrimary && melee.IsPressed()) || (!canHoldPrimary && melee.WasPerformedThisFrame())))
            {
                TryPrimary();
            }

            //if (brain.hasProjectileWeapon && ((canHoldSecondary && projectile.IsPressed()) || (!canHoldSecondary && projectile.WasPerformedThisFrame())))
            //{
            //    TrySecondary();
            //}
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
