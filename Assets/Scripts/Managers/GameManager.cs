using TMPro.EditorUtilities;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerData playerData;

    void Start()
    {
        // Load player data when the game starts
        playerData = SaveLoadManager.LoadPlayerData();

        // Apply the loaded data to the game (e.g., cash, settings, etc.)
        ApplyPlayerData();
    }

    void OnApplicationQuit()
    {
        // Save player data when the game is closed
        SaveLoadManager.SavePlayerData(playerData);
    }

    void ApplyPlayerData()
    { 
        // For example:
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

    // This method might be called when the player earns cash or unlocks something
    public void UpdatePlayerData(int cashChange, int unlockChange)
    {
        playerData.cash += cashChange;
        playerData.unlocks += unlockChange;

        // Save after making changes
        SaveLoadManager.SavePlayerData(playerData);
    }
}
