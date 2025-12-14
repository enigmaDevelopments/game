using UnityEngine;

public class AntennaBrain : CharacterBrain
{
    public enum Pack : byte
    {
        None,
        laserPack,
        sheildPack,
        pulsePack,
        jetPack
    }
    public enum Gun : byte
    {
        None,
        pistol,
        minigun
    }
    public Gun defultGun = Gun.pistol;
    public Pack defultPack;
    [Header("Guns")]
    public GameObject pistolObject;
    public GameObject miniGunObject;
    [Header("Pack Objects")]
    public GameObject laserPackObject;
    public GameObject pulsePackObject;
    public GameObject sheildPackObject;
    public GameObject jetPackObject;

    private bool _pistol = false;
    private bool _minigun = false;
    private bool _laserPack = false;
    private bool _pulsePack = false;
    private bool _sheildPack = false;
    private bool _jetPack = false;

    private GameObject jetPackInstance;

    #region guns
    public bool pistol
    {
        get { return _pistol; }
        set
        {
            if (value)
            {
                minigun = false;
                SetProjectileWeapon(pistolObject);
            }
            else if (_laserPack)
            {
                RemoveProjectileWeapon();
            }
            _pistol = value;
        }
    }
    public bool minigun
    {
        get { return _minigun; }
        set
        {
            if (value)
            {
                pistol = false;
                SetProjectileWeapon(miniGunObject);
            }
            else if (_pistol)
            {
                RemoveProjectileWeapon();
            }
            _minigun = value;
        }
    }
    #endregion

    #region packs
    public bool laserPack
    {
        get { return _laserPack; }
        set 
        {
            if (value)
            {
                pulsePack = false;
                sheildPack = false;
                jetPack = false;
                SetSpecialWepon(laserPackObject);
            }
            else if (_laserPack)
            {
                RemoveSpecialWepon();
            }
            _laserPack = value;
        }
    }

    public bool pulsePack
    {
        get { return _pulsePack; }
        set
        {
            if (value)
            {
                laserPack = false;
                sheildPack = false;
                jetPack = false;
                SetSpecialWepon(pulsePackObject);
            }
            else if (_pulsePack)
            {
                RemoveSpecialWepon();
            }
            _pulsePack = value;
        }
    }

    public bool sheildPack
    {
        get { return _sheildPack; }
        set
        {
            if (value)
            {
                laserPack = false;
                pulsePack = false;
                jetPack = false;
                SetSpecialWepon(sheildPackObject);
            }
            else if (_sheildPack)
            {
                RemoveSpecialWepon();
            }
            _sheildPack = value;
        }
    }

    public bool jetPack
    {
        get { return _jetPack; }
        set
        {
            if (value)
            {
                laserPack = false;
                pulsePack = false;
                sheildPack = false;
                jetPack = false;
                _hasSpecialeWeapon = false;

                jetPackInstance = Instantiate(jetPackObject,specialWeponLocation);
                GetComponent<Fly>().enabled = true;
                GetComponent<ThirdPersonMovement>().enabled = false;
            }
            else if (_jetPack)
            {
                Destroy(jetPackInstance);
                GetComponent<ThirdPersonMovement>().enabled = true;
                GetComponent<Fly>().enabled = false;
            }
            _jetPack = value;
        }
    }

    public void RemovePack()
    {
        _laserPack = false;
        _pulsePack = false;
        _sheildPack = false;
        _jetPack = false;
        RemoveSpecialWepon();
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        if (defultGun == Gun.pistol)
            pistol = true;
        else if (defultGun == Gun.minigun)
            minigun = true;
        if (defultPack == Pack.laserPack)
            laserPack = true;
        else if (defultPack == Pack.pulsePack)
            pulsePack = true;
        else if (defultPack == Pack.sheildPack)
            sheildPack = true;
        else if (defultPack == Pack.jetPack)
            jetPack = true;
    }
}

