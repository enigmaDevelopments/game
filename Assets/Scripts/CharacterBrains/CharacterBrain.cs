using UnityEngine;

public class CharacterBrain : MonoBehaviour
{
    public AttackController attackController;
    [Header("Settings")]
    public bool hasMeleeWeapon;
    public bool hasProjectileWeapon;
    public bool hasSpecial;
    public bool hasDash;
}
