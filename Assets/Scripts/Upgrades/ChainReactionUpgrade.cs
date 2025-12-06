using UnityEngine;

public class ChainReactionUpgrade : MonoBehaviour
{
    public bool isActive = false;  // If the upgrade is active or not
    public float triggerChance = 0.5f;    // Chance to trigger a chain reaction on kill

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Activate();
            Destroy(gameObject);  // Destroy the upgrade item after it’s picked up
        }
    }

    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            Debug.Log("Chain Reaction Upgrade Activated!");
        }
    }

    public void TryTriggerChainReaction(Enemy enemy)
    {
        if (!isActive) return;

        if (Random.value <= triggerChance)
        {
            var chainReaction = enemy.GetComponent<ChainReaction>();
            if (chainReaction != null)
            {
                chainReaction.TriggerChainReaction();
            }
        }
    }
}
