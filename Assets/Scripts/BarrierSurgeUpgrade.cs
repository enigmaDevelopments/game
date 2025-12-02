using UnityEngine;
using System.Collections;  // Make sure this is here for IEnumerator


public class BarrierSurgeUpgrade : MonoBehaviour
{
    public float duration = 10f; // How long the barrier surge lasts
    public float maxBarrierIncrease = 150f;  // How much more barrier the player gets

    private BarrierScript playerBarrier;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerBarrier = other.GetComponent<BarrierScript>();
            if (playerBarrier != null)
            {
                ActivateBarrierSurge();
                Destroy(gameObject);  // Destroy the upgrade item after it's picked up
            }
        }
    }

    public void ActivateBarrierSurge()
    {
        // Increase the player's max barrier
        playerBarrier.maxBarrier += maxBarrierIncrease;
        playerBarrier.ActivateBarrier();

        // Deactivate the barrier surge after the set duration
        StartCoroutine(DeactivateBarrierSurgeAfterTime());
    }

    private IEnumerator DeactivateBarrierSurgeAfterTime()
    {
        yield return new WaitForSeconds(duration);
        playerBarrier.DeactivateBarrier();
        playerBarrier.maxBarrier -= maxBarrierIncrease;  // Reset the max barrier back to normal
        Debug.Log("Barrier Surge Expired");
    }
}
