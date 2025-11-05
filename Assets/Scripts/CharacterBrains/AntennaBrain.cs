using UnityEngine;

public class AntennaBrain : CharacterBrain
{
    [Header("Pack Objects")]
    public GameObject laserPackObject;
    public GameObject pulsePackObject;
    public GameObject sheildPackObject;

    private bool _laserPack = false;
    private bool _pulsePack = false;
    private bool _sheildPack = false;
    private bool _jetPack = false;

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
                SetSpecialWepon(laserPackObject);
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
                SetSpecialWepon(laserPackObject);
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
                
                GetComponent<Fly>().enabled = true;
                GetComponent<ThirdPersonMovement>().enabled = false;
            }
            else if (_jetPack)
            {
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
}

