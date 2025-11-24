using UnityEngine;

public class Coins : MonoBehaviour
{
    public int amount = 1; // how much currency this coin gives

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Example: player has a CurrencyManager component
        var currency = other.GetComponent<CurrencyManager>();
        if (currency != null)
        {
            currency.AddCoins(amount);
            Debug.Log($"Picked up {amount} coins!");
        }

        Destroy(gameObject);
    }
}
