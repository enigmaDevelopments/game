using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AttackController : MonoBehaviour
{
    [System.Serializable]
    public class AiSettings
    {
        [Range(0f, 180f)]
        public float attackAngle;
        public bool canAttackWhileMoving;
        [SerializeField]
        public Transform rotationTransform;
        public Transform weponTransform;
        public Vector3 rotationOffsetAngle;
        [NonSerialized]
        public AttackBase attack;
        [NonSerialized]
        public Quaternion rotationOffset;
        [NonSerialized]
        public bool canUse;
    }



    [Header("Control Mode")]
    [Tooltip("If true, attacks are controlled by AI instead of player input")]
    public bool isAIControlled = false;
    [SerializeField]
    private AiSettings primaryAttackSettings;
    [SerializeField]
    private AiSettings secondaryAttackSettings;
    [SerializeField]
    private AiSettings tertiaryAttackSettings;


    [Header("Scripts")]
    [Tooltip("Optional. If not provided, will try to GetComponent<PlayerInput>()")]
    public PlayerInput input;

    [Tooltip("Optional. If not provided, will try to GetComponent<CharacterBrain>()")]
    public CharacterBrain brain;

    [Header("Attacks")]
    public AttackBase primaryAttack;   // Melee
    public AttackBase secondaryAttack; // Projectile
    public AttackBase tertiaryAttack; // special
    [Header("Animations")]
    public WeponAnimation primaryAnimation;   // Melee
    public WeponAnimation secondaryAnimation; // Projectile
    public WeponAnimation tertiaryAnimation; // special

    public AiSettings[] attacks
    {
        get
        {
            AiSettings[] output = new AiSettings[3];
            output[0] = primaryAttackSettings;
            output[1] = secondaryAttackSettings;
            output[2] = tertiaryAttackSettings;
            output[0].attack = primaryAttack;
            output[1].attack = secondaryAttack;
            output[2].attack = tertiaryAttack;
            output[0].rotationOffset = Quaternion.Inverse(Quaternion.Euler(output[0].rotationOffsetAngle));
            output[1].rotationOffset = Quaternion.Inverse(Quaternion.Euler(output[1].rotationOffsetAngle));
            output[2].rotationOffset = Quaternion.Inverse(Quaternion.Euler(output[2].rotationOffsetAngle));
            output[0].canUse = CanUsePrimary();
            output[1].canUse = CanUseSecondary();
            output[2].canUse = CanUseTertiary();
            return output;
        }
    }

    private void Awake()
    {
        if (input == null)
            input = GetComponent<PlayerInput>();
        if (brain == null)
            brain = GetComponent<CharacterBrain>();
    }


    private void Start()
    {
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
            return;
        // Player input mode
        if (input != null)
        {
            var actions = input.actions;
            var melee = actions?.FindAction("MeleeAttack", throwIfNotFound: false);
            var projectile = actions?.FindAction("ProjectileAttack", throwIfNotFound: false);
            var special = actions?.FindAction("Special", throwIfNotFound: false);

            if (brain.hasMeleeWeapon && ((primaryAttack.CanHold && melee.IsPressed()) || melee.WasPerformedThisFrame()))
                TryPrimary();
            if (brain.hasProjectileWeapon && ((secondaryAttack.CanHold && projectile.IsPressed()) || projectile.WasPerformedThisFrame()))
                TrySecondary();
            if (brain.hasSpecialeWeapon && ((tertiaryAttack.CanHold && special.IsPressed()) || special.WasPerformedThisFrame()))
                TryTertiary();
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
        return primaryAttack != null && !primaryAttack.IsAttacking;
    }

    public bool CanUseSecondary()
    {
        return secondaryAttack != null && !secondaryAttack.IsAttacking;
    }

    public bool CanUseTertiary()
    {
        return tertiaryAttack != null && !tertiaryAttack.IsAttacking;
    }
}
