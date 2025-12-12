using UnityEngine;
using UnityEngine.UI;

public class DamageUpgrade : MonoBehaviour
{
    [Header("Upgrade Settings")]
    [SerializeField] private float damageMultiplier = 2f;  // How much to boost damage
    [SerializeField] private int coinThreshold = 500;      // Coins needed to activate
    [SerializeField] private Text upgradeStatusText;       // Optional UI text

    [Header("References")]
    [SerializeField] private AttackBase primaryAttack;         // Assign primary weapon here
    [SerializeField] private CurrencyManager currencyManager;  // Assign your currency manager

    private float originalDamage;   // Stores the original base damage
    public bool isActive = false;

    private void Start()
    {
        if (primaryAttack != null)
        {
            // ?? Use the property we just added on AttackBase
            originalDamage = primaryAttack.CurrentDamage;
        }
    }

    private void Update()
    {
        if (primaryAttack == null || currencyManager == null) return;

        int coins = currencyManager.GetCoinCount();

        // Activate upgrade if coins are enough
        if (!isActive && coins >= coinThreshold)
        {
            primaryAttack.SetDamage(originalDamage * damageMultiplier);
            isActive = true;

            if (upgradeStatusText != null)
                upgradeStatusText.text = "Damage Boost Active!";
        }
        // Deactivate upgrade if coins drop below threshold
        else if (isActive && coins < coinThreshold)
        {
            primaryAttack.SetDamage(originalDamage);
            isActive = false;

            if (upgradeStatusText != null)
                upgradeStatusText.text = $"Collect {coinThreshold}+ coins for damage boost!";
        }
    }
}