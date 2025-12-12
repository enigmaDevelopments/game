//using UnityEngine;
//using UnityEngine.UI;

//// script will double players damage ability if they hit 500 coins
//public class DamageUpgrade : MonoBehaviour
//{
//    public float damageMultiplier = 2f;  // The multiplier to apply when the player has more than 500 coins
//    private PlayerAttack playerAttack;   // Reference to the PlayerAttack component
//    private CurrencyManager currencyManager;  // Reference to the CurrencyManager component
//    public Text upgradeStatusText;   // UI Text to show the upgrade status

//    [Header("Coin Threshold for Upgrade")]
//    public int coinThreshold = 500;  // Coin threshold for the upgrade to activate

//    private float originalDamage;  // Store the original damage value to revert back when upgrade is deactivated

//    private void Start()
//    {
//        playerAttack = FindAnyObjectByType<PlayerAttack>();  // Get the player attack script
//        currencyManager = FindAnyObjectByType<CurrencyManager>();  // Get the currency manager

//        // Store the original damage value at the start
//        if (playerAttack != null)
//        {
//            originalDamage = playerAttack.baseDamage;
//        }
//    }

//    private void Update()
//    {
//        if (currencyManager != null && playerAttack != null)
//        {
//            // Check if the player has more than 500 coins
//            if (currencyManager.GetCoinCount() > coinThreshold)
//            {
//                // Double the player's damage if more than 500 coins
//                if (playerAttack.baseDamage != originalDamage * damageMultiplier)
//                {
//                    playerAttack.baseDamage = originalDamage * damageMultiplier;
//                    Debug.Log("Damage Upgrade Active! Damage is now doubled.");

//                    // Update UI text
//                    if (upgradeStatusText != null)
//                        upgradeStatusText.text = "Damage Doubled!";
//                }
//            }
//            else
//            {
//                // Revert to original damage if below the threshold
//                if (playerAttack.baseDamage != originalDamage)
//                {
//                    playerAttack.baseDamage = originalDamage;
//                    Debug.Log("Damage Upgrade Deactivated! Damage reverted.");

//                    // Update UI text
//                    if (upgradeStatusText != null)
//                        upgradeStatusText.text = "Collect more coins to double damage!";
//                }
//            }
//        }
//    }
//}
using UnityEngine;
using UnityEngine.UI;

public class DamageUpgrade : MonoBehaviour
{
    [Header("Upgrade Settings")]
    [SerializeField] private float damageMultiplier = 2f;  // How much to boost damage
    [SerializeField] private int coinThreshold = 500;      // Coins needed to activate
    [SerializeField] private Text upgradeStatusText;       // Optional UI text

    [Header("References")]
    [SerializeField] private AttackBase primaryAttack;    // Assign primary weapon here
    [SerializeField] private CurrencyManager currencyManager; // Assign your currency manager

    private float originalDamage;   // Stores the original base damage
    public bool isActive = false;

    private void Start()
    {
        // Store original damage from the primary weapon
        if (primaryAttack != null)
        {
            originalDamage = primaryAttack.Damage;
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
