using UnityEngine;

public class AntennaBrain : CharacterBrain
{
    [Header("Pack Objects")]
    public GameObject laserPackObject;
    public GameObject pulsePackObject;
    public GameObject sheildPackObject;
    public GameObject jetPackObject;

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
                activePack = Instantiate(laserPackObject,transform);
            }
            else if (_laserPack)
                Destroy(activePack);
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
                activePack = Instantiate(pulsePackObject,transform);
            }
            else if (_pulsePack) 
                Destroy(activePack);
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
                activePack = Instantiate(sheildPackObject,transform);
            }
            else if (_sheildPack)
                Destroy (activePack);
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
                activePack = Instantiate(jetPackObject,transform);
            }
            else if (_jetPack)
                Destroy (activePack);
            _jetPack = value;
        }
    }
    #endregion
}

