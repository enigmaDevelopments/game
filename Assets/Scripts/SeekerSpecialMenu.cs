using UnityEngine;

public class SeekerSpecialMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SeekerBrain seekerBrain;

    public void SetBrain(SeekerBrain brain)
    {
        seekerBrain = brain;
    }

    private void Awake()
    {
        // Auto-find if not assigned in Inspector
        if (seekerBrain == null)
        {
            seekerBrain = FindAnyObjectByType<SeekerBrain>();
            if (seekerBrain == null)
            {
                Debug.LogError("SeekerSpecialMenu: No SeekerBrain found in scene.");
            }
        }
    }

    public void EquipInvisabiltyChip()
    {
        if (seekerBrain == null) return;
        seekerBrain.invisabilityChip = true;    
    }

    public void EquipGrapplingHook()
    {
        if (seekerBrain == null) return;
        seekerBrain.grapplingHook = true;
    }
}
