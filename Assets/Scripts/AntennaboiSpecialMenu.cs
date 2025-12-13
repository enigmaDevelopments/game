using UnityEngine;

public class AntennaboiSpecialMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AntennaBrain antennaBrain;

    public void SetBrain(AntennaBrain brain)
    {
        antennaBrain = brain;
    }

    private void Awake()
    {
        // Auto-find if not assigned in Inspector
        if (antennaBrain == null)
        {
            antennaBrain = FindAnyObjectByType<AntennaBrain>();
            if (antennaBrain == null)
            {
                Debug.LogError("AntennaboiSpecialMenu: No AntennaBrain found in scene.");
            }
        }
    }

    public void EquipLaserPack()
    {
        if (antennaBrain == null) return;
        antennaBrain.laserPack = true;    // this will turn others off and call SetSpecialWepon(laserPackObject)
    }

    public void EquipPulsePack()
    {
        if (antennaBrain == null) return;
        antennaBrain.pulsePack = true;
    }

    public void EquipShieldPack()
    {
        if (antennaBrain == null) return;
        antennaBrain.sheildPack = true;
    }

    public void EquipJetPack()
    {
        if (antennaBrain == null) return;
        antennaBrain.jetPack = true;
    }

    public void RemovePack()
    {
        if (antennaBrain == null) return;
        antennaBrain.RemovePack();
    }
}
