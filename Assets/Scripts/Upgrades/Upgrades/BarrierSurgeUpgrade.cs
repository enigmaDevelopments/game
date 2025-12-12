using UnityEngine;
using System.Collections;

public class BarrierSurgeUpgrade : MonoBehaviour
{
    public float duration = 10f;
    public float maxBarrierIncrease = 150f;

    private BarrierScript playerBarrier;
    public bool isActive = false;
    private bool activatedThisBarrier = false;

    public void Activate(BarrierScript targetBarrier)
    {
        if (isActive)
            return;

        if (targetBarrier == null)
        {
            Debug.LogError("BarrierSurgeUpgrade: No barrier assigned!");
            return;
        }

        playerBarrier = targetBarrier;

        // increase max barrier
        playerBarrier.maxBarrier += maxBarrierIncrease;
        playerBarrier.ClampBarrier();

        // activate barrier only if it wasn’t active already
        if (!playerBarrier.IsBarrierActive)
        {
            playerBarrier.ActivateBarrier();
            activatedThisBarrier = true;
        }

        isActive = true;

        Debug.Log("Barrier Surge Upgrade Activated!");
        StartCoroutine(DeactivateAfterTime());
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(duration);

        if (playerBarrier != null)
        {
            // remove max barrier increase
            playerBarrier.maxBarrier -= maxBarrierIncrease;
            playerBarrier.ClampBarrier();

            // only deactivate if THIS upgrade turned it on
            if (activatedThisBarrier)
                playerBarrier.DeactivateBarrier();

            Debug.Log("Barrier Surge Upgrade Expired.");
        }

        isActive = false;
        activatedThisBarrier = false;
    }
}
