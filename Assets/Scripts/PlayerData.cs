[System.Serializable]
public class PlayerData
{
    public int cash;
    public int unlocks; // You can use this to store a bitmask or an int representing unlocked content
    public string currentPlayer;
    public PlayerSettings settings; // You can store settings like volume, controls, etc.

    // Constructor for initializing default values
    public PlayerData()
    {
        cash = 0;
        unlocks = 0;
        currentPlayer = "Default";
        settings = new PlayerSettings(); // For example, volume, brightness, etc.
    }
}
