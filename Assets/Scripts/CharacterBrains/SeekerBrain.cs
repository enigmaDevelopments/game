using Unity.VisualScripting;
using UnityEngine;

public class SeekerBrain : CharacterBrain
{
    public enum Weapons : byte
    {
        None,
        invisabilityChip,
        grapplingHook
    }
    public enum Gun : byte
    {
        None,
        sniper,
        laserRifle
    }

    public Gun defultGun = Gun.sniper;
    public Weapons defaultWeapons;

    public GameObject sniperObject;
    public GameObject laserRifleObject;
    public GameObject invisabilityChipObject;
    public GameObject grapplingHookObject;

    private bool _sniper = false;
    private bool _laserRifle = false;
    private bool _invisabilityChip = false;
    private bool _grapplingHook = false;

    public bool sniper
    {
        get { return _sniper; }
        set
        {
            if (value)
            {
                laserRifle = false;
                SetProjectileWeapon(sniperObject);
            }
            else if (_sniper)
            {
                RemoveProjectileWeapon();
            }
            _sniper = value;
        }
    }
    public bool laserRifle
    {
        get { return _laserRifle; }
        set
        {
            if (value)
            {
                sniper = false;
                SetProjectileWeapon(laserRifleObject);
            }
            else if (_laserRifle)
            {
                RemoveProjectileWeapon();
            }
            _laserRifle = value;
        }
    }

    public bool invisabilityChip
    {
        get { return _invisabilityChip; }
        set
        {
            if (value)
            {
                _grapplingHook = false;
           
                SetSpecialWepon(invisabilityChipObject);
            }
            else if (_invisabilityChip)
            {
                RemoveSpecialWepon();
            }
            _invisabilityChip = value;
        }
    }

    public bool grapplingHook
    {
        get { return _grapplingHook; }
        set
        {
            if (value)
            {
                invisabilityChip = false;
               
                SetSpecialWepon(grapplingHookObject);
            }
            else if (_grapplingHook)
            {
                RemoveSpecialWepon();
            }
            _grapplingHook = value;
        }
    }

    public void RemoveWeapon()
    {
        _invisabilityChip = false;
        _grapplingHook = false;
        
        RemoveSpecialWepon();
    }

    protected override void Awake()
    {
        base.Awake();
        if (defultGun == Gun.sniper)
            sniper = true;
        else if (defultGun == Gun.laserRifle)
            laserRifle = true;
        if (defaultWeapons == Weapons.invisabilityChip)
            invisabilityChip = true;
        else if (defaultWeapons == Weapons.grapplingHook)
            grapplingHook = true;
       
    }


}
