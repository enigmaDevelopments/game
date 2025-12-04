//Attache scprit to player

using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public int coins = 0;
    public int currentCoins = 0;

    public void AddCoins(int amount)
    {
        coins += amount;
        // Optional: update UI here
    }
    public void RemoveCoins(int amount)
    {
        currentCoins -= amount;
        currentCoins = Mathf.Max(currentCoins, 0);  // Prevent negative coin count
        Debug.Log($"Coins removed! Current coin count: {currentCoins}");
    }

    public int GetCoinCount()
    {
        return currentCoins;  // Return the current coin count
    }
}
