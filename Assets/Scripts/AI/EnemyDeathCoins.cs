//Call Die() to initiate
using UnityEngine;

public class EnemyDeathCoins: MonoBehaviour
{
    public GameObject coinPrefab; // Drag and drop your coin prefab here in the Inspector
    public int coinsDropped = 3;  // How many coins the enemy drops on death

    public void Die()
    {
        DropCoins();
        Destroy(gameObject);
    }

    private void DropCoins()
    {
        for (int i = 0; i < coinsDropped; i++)
        {
            // Instantiate coin at enemy's position with no rotation
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
    }
}
