//Attache scprit to player

using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public int coins = 0;

    public void AddCoins(int amount)
    {
        coins += amount;
        // Optional: update UI here
    }
}
