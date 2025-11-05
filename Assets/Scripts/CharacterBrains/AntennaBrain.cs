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
    private GameObject activePack;

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
                hasSpecial = true;
                activePack = Instantiate(laserPackObject, transform);
                attackController.tertiaryAttack = activePack.GetComponent<AttackBase>();
            }
            else if (_laserPack)
            {
                hasSpecial = false;
                Destroy(activePack);
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
                hasSpecial = true;
                activePack = Instantiate(pulsePackObject,transform);
                attackController.tertiaryAttack = activePack.GetComponent<AttackBase>();
            }
            else if (_pulsePack)
            {
                hasSpecial = false;
                Destroy(activePack);
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
                hasSpecial = true;
                activePack = Instantiate(sheildPackObject,transform);
                attackController.tertiaryAttack = activePack.GetComponent<AttackBase>();
            }
            else if (_sheildPack)
            {
                hasSpecial = false;
                Destroy(activePack);
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
                hasSpecial = false;
                
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
    #endregion
}

