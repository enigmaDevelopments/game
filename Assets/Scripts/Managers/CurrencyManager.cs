using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public int coins = 0;               // Total coins earned across sessions
    public int currentCoins = 0;        // Coins available for spending

    private void Awake()
    {
        // Load saved player data if available
        PlayerData data = SaveLoadManager.LoadPlayerData();
        if (data != null)
        {
            coins = data.cash;
            currentCoins = data.cash;
        }
    }

    /// <summary>
    /// Add coins to the player
    /// </summary>
    /// <param name="amount">Amount to add</param>
    public void AddCoins(int amount)
    {
        coins += amount;
        currentCoins += amount;
        UpdateUI();
        SaveData();
        Debug.Log($"Added {amount} coins. Current coins: {currentCoins}");
    }

    /// <summary>
    /// Spend coins if enough are available
    /// </summary>
    /// <param name="amount">Amount to spend</param>
    /// <returns>True if coins were spent, false if not enough coins</returns>
    public bool SpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            UpdateUI();
            SaveData();
            Debug.Log($"Spent {amount} coins. Remaining coins: {currentCoins}");
            return true;
        }

        Debug.LogWarning($"Not enough coins to spend {amount}. Current coins: {currentCoins}");
        return false;
    }

    /// <summary>
    /// Get current coins available for spending
    /// </summary>
    /// <returns>Current coin count</returns>
    public int GetCoinCount()
    {
        return currentCoins;
    }

    /// <summary>
    /// Updates the UI in UpgradeManager if available
    /// </summary>
    private void UpdateUI()
    {
        UpgradeManager upgradeManager = FindAnyObjectByType<UpgradeManager>();
        if (upgradeManager != null)
        {
            upgradeManager.UpdateCurrencyDisplay();
        }
    }

    /// <summary>
    /// Save player data via GameManager
    /// </summary>
    private void SaveData()
    {
        GameManager gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            // Update cash in playerData and save
            gameManager.UpdatePlayerData(0, 0);
        }
    }
}
