using UnityEngine;

public class ChainReactionUpgrade : MonoBehaviour
{
    private bool isUpgradeActive = false;  // If the upgrade is active or not
    public float triggerChance = 0.5f;    // Chance to trigger a chain reaction on kill

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateChainReactionUpgrade();
            Destroy(gameObject);  // Destroy the upgrade item after it’s picked up
        }
    }

   public void ActivateChainReactionUpgrade()
    {
        isUpgradeActive = true;
        Debug.Log("Chain Reaction Upgrade Activated!");
    }

    public void TryTriggerChainReaction(Enemy enemy)
    {
        if (isUpgradeActive && Random.value <= triggerChance)
        {
            // Trigger chain reaction when the player kills an enemy
            var chainReaction = enemy.GetComponent<ChainReaction>();
            if (chainReaction != null)
            {
                chainReaction.TriggerChainReaction();
            }
        }
    }
}
