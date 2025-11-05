using UnityEngine;

public class CharacterBrain : MonoBehaviour
{
    public AttackController attackController;
    [Header("Settings")]
    public bool hasMeleeWeapon => _hasMeleeWeapon;
    public bool hasProjectileWeapon => _hasProjectileWeapon;
    public bool hasSpecialeWeapon => _hasSpecialeWeapon;
    public bool hasDash => _hasDash;

    protected bool _hasMeleeWeapon;
    protected bool _hasProjectileWeapon;
    protected bool _hasSpecialeWeapon;
    protected bool _hasDash;
    private GameObject activeMeleeWeapon;
    private GameObject activeProjectileWeapon;
    private GameObject activeSpecialeWeapon;

    public void SetMeleeWeapon(GameObject wepon)
    {
        _hasMeleeWeapon = true;
        activeMeleeWeapon = Instantiate(wepon, transform);
        attackController.primaryAttack = activeMeleeWeapon.GetComponent<AttackBase>();
    }
    public void SetProjectileWeapon(GameObject wepon)
    {
        _hasProjectileWeapon = true;
        activeProjectileWeapon = Instantiate(wepon, transform);
        attackController.secondaryAttack = activeProjectileWeapon.GetComponent<AttackBase>();
    }
    public void SetSpecialWepon(GameObject wepon)
    {
        _hasSpecialeWeapon = true;
        activeSpecialeWeapon = Instantiate(wepon, transform);
        attackController.tertiaryAttack = activeSpecialeWeapon.GetComponent<AttackBase>();
    }

    public void RemoveMeleeWeapon()
    {
        _hasMeleeWeapon = false;
        Destroy(activeMeleeWeapon);
    }
    public void RemoveProjectileWeapon()
    {
        _hasProjectileWeapon = false;
        Destroy(activeProjectileWeapon);
    }
    public void RemoveSpecialWepon()
    {
        _hasSpecialeWeapon = false;
        Destroy(activeSpecialeWeapon);
    }
}