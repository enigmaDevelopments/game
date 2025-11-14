using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private static string saveFilePath => Application.persistentDataPath + "/playerData.json";

    // Save data to a file
    public static void SavePlayerData(PlayerData playerData)
    {
        string json = JsonUtility.ToJson(playerData, true); // Convert data to JSON format
        File.WriteAllText(saveFilePath, json); // Write JSON to disk
        Debug.Log("Player data saved to: " + saveFilePath);
    }

    // Load data from the file
    public static PlayerData LoadPlayerData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath); // Read the JSON from disk
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(json); // Convert JSON to object
            Debug.Log("Player data loaded from: " + saveFilePath);
            return playerData;
        }
        else
        {
            Debug.LogWarning("No save file found, creating a new one.");
            return new PlayerData(); // Return a new player data if no file exists
        }
    }
}
