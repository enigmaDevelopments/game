using UnityEngine;

public class CharacterBrain : MonoBehaviour
{
    public AttackController attackController;
    [Header("Locations")]
    public Transform meleeWeponLocation;
    public Transform projectileWeponLocation;
    public Transform specialWeponLocation;
    [Header("Defults")]
    public GameObject defultMeleeWeapon;
    public GameObject defultProjectileWeapon;
    public GameObject defultSpecialWeapon;

    public bool hasMeleeWeapon => _hasMeleeWeapon;
    public bool hasProjectileWeapon => _hasProjectileWeapon;
    public bool hasSpecialeWeapon => _hasSpecialeWeapon;

    protected bool _hasMeleeWeapon;
    protected bool _hasProjectileWeapon;
    protected bool _hasSpecialeWeapon;
    private GameObject activeMeleeWeapon;
    private GameObject activeProjectileWeapon;
    private GameObject activeSpecialeWeapon;

    public void SetMeleeWeapon(GameObject wepon)
    {
        _hasMeleeWeapon = true;
        activeMeleeWeapon = Instantiate(wepon, meleeWeponLocation);
        attackController.primaryAttack = activeMeleeWeapon.GetComponent<AttackBase>();
    }
    public void SetProjectileWeapon(GameObject wepon)
    {
        _hasProjectileWeapon = true;
        activeProjectileWeapon = Instantiate(wepon, projectileWeponLocation);
        attackController.secondaryAttack = activeProjectileWeapon.GetComponent<AttackBase>();
    }
    public void SetSpecialWepon(GameObject wepon)
    {
        _hasSpecialeWeapon = true;
        activeSpecialeWeapon = Instantiate(wepon, specialWeponLocation);
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
    protected virtual void Start()
    {
        if (defultMeleeWeapon != null) 
            SetMeleeWeapon(defultMeleeWeapon);
        if (defultProjectileWeapon != null)
            SetProjectileWeapon(defultProjectileWeapon);
        if (defultSpecialWeapon != null)
            SetSpecialWepon(defultSpecialWeapon);
    }
}