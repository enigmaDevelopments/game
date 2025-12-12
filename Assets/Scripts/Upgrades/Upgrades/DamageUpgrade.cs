using UnityEngine;
using UnityEngine.UI;

public class DamageUpgrade : MonoBehaviour
{
    [Header("Upgrade Settings")]
    [SerializeField] private float damageMultiplier = 2f;   // Double damage
    [SerializeField] private int coinThreshold = 500;
    [SerializeField] private Text upgradeStatusText;

    [Header("References")]
    [SerializeField] private CurrencyManager currencyManager;

    private WeaponDamageWrapper[] weapons;  // all weapons found
    private bool isActive = false;

    private void Start()
    {
        // Find all weapon damage components attached to the player or scene
        weapons = Object.FindObjectsByType<WeaponDamageWrapper>(
        FindObjectsInactive.Exclude,
        FindObjectsSortMode.None);

        Debug.Log("DamageUpgrade detected " + weapons.Length + " weapons.");
    }

    private void Update()
    {
        if (currencyManager == null) return;

        int coins = currencyManager.GetCoinCount();

        if (!isActive && coins >= coinThreshold)
            ActivateUpgrade();
        else if (isActive && coins < coinThreshold)
            DeactivateUpgrade();
    }

    private void ActivateUpgrade()
    {
        foreach (var w in weapons)
            w.ApplyMultiplier(damageMultiplier);

        isActive = true;

        if (upgradeStatusText != null)
            upgradeStatusText.text = "Damage Boost Active!";
    }

    private void DeactivateUpgrade()
    {
        foreach (var w in weapons)
            w.RestoreOriginalDamage();

        isActive = false;

        if (upgradeStatusText != null)
            upgradeStatusText.text = $"Collect {coinThreshold}+ coins for damage boost!";
    }
}
