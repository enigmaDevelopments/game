using UnityEngine;
using static AntennaBrain;

public class SeekerBrain : CharacterBrain
{
    public enum Weapons : byte
    {
        None,
        invisabilityChip,
        grapplingHook
    }
    public Weapons defaultWeapons;

    public GameObject invisabilityChipObject;
    public GameObject grapplingHookObject;

    private bool _invisabilityChip = false;
    private bool _grapplingHook = false;

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
        if (defaultWeapons == Weapons.invisabilityChip)
            invisabilityChip = true;
        else if (defaultWeapons == Weapons.grapplingHook)
            grapplingHook = true;
       
    }


}
