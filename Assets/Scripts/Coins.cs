using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField] private int value = 10; // $10 per coin

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CurrencyManager.Instance != null)
            {
                CurrencyManager.Instance.AddMoney(value);
            }

            // Optionally play sound / VFX here

            Destroy(gameObject);
        }
    }
}
