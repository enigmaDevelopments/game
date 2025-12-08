using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private PlayerData playerData;

    // The currently active player in the scene
    public GameObject currentPlayer;


    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        // Load player data at game start
        playerData = SaveLoadManager.LoadPlayerData();

        ApplyPlayerData();
    }


    private void OnApplicationQuit()
    {
        // Save player data when closing game
        SaveLoadManager.SavePlayerData(playerData);
    }


    private void ApplyPlayerData()
    {
        Debug.Log("Loaded Cash: " + playerData.cash);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetVolume(playerData.settings.volume);
        }
        else
        {
            Debug.LogWarning("AudioManager not found!");
        }
    }


    // ------------------------------------------------------------------------------------
    // ?? Register Player (this connects the player to the UpgradeManager)
    // ------------------------------------------------------------------------------------
    public void RegisterPlayer(GameObject player)
    {
        currentPlayer = player;

        Debug.Log("GameManager: Player registered ? " + player.name);

        // Also update the UpgradeManager
        UpgradeManager upgradeManager = FindAnyObjectByType<UpgradeManager>();
        if (upgradeManager != null)
        {
            upgradeManager.RegisterPlayer(player);
        }
        else
        {
            Debug.LogWarning("UpgradeManager not found in scene!");
        }
    }


    // ------------------------------------------------------------------------------------
    // ?? Update Player Data (cash, unlocks, etc.)
    // ------------------------------------------------------------------------------------
    public void UpdatePlayerData(int cashChange, int unlockChange)
    {
        playerData.cash += cashChange;
        playerData.unlocks += unlockChange;

        SaveLoadManager.SavePlayerData(playerData);
    }
}
