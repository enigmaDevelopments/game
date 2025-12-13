using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }

    [System.Serializable]
    public class PlayerData
    {
        public Vector3 position;
        public float health;
        public int coins;
    }

    private PlayerData currentPlayerData;

    private void Awake()
    {
        // Singleton pattern - only one instance across scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Save player data before loading a new level
    /// </summary>
    public void SavePlayerData(GameObject player, int coins)
    {
        if (player == null) return;

        currentPlayerData = new PlayerData
        {
            position = player.transform.position,
            coins = coins,
        };

        // Get player health
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            currentPlayerData.health = playerHealth.currentHealth;
        }

        Debug.Log($"Player data saved! Health: {currentPlayerData.health}, Coins: {currentPlayerData.coins}, Position: {currentPlayerData.position}");
    }

    /// <summary>
    /// Load player data in a new level
    /// </summary>
    public void LoadPlayerData(GameObject player, CurrencyManager currencyManager)
    {
        if (player == null || currentPlayerData == null) return;

        // Restore position
        player.transform.position = currentPlayerData.position;

        // Restore health
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.currentHealth = currentPlayerData.health;
        }

        // Restore coins
        if (currencyManager != null)
        {
            currencyManager.AddMoney(currentPlayerData.coins);
        }

        Debug.Log($"Player data loaded! Health: {currentPlayerData.health}, Coins: {currentPlayerData.coins}");
    }

    /// <summary>
    /// Clear saved data (for new game)
    /// </summary>
    public void ClearPlayerData()
    {
        currentPlayerData = null;
        Debug.Log("Player data cleared.");
    }

    public bool HasSavedData() => currentPlayerData != null;
}

