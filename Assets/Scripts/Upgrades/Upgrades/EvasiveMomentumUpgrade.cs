using UnityEngine;
using System.Collections;

public class EvasiveMomentumUpgrade : MonoBehaviour
{
    [Header("Evasive Momentum Settings")]
    [Tooltip("How much to multiply the player's speed when triggered")]
    public float speedMultiplier = 1.5f;

    [Tooltip("How long the speed boost lasts (in seconds)")]
    public float boostDuration = 2f;

    [Tooltip("Cooldown before the boost can trigger again")]
    public float cooldown = 5f;

    [Tooltip("Enable or disable this upgrade")]
    public bool isActive = false;

    private ThirdPersonMovement playerMovement;
    private bool canTrigger = true;

    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerMovement = player.GetComponent<ThirdPersonMovement>();
            }

            Debug.Log("Evasive Momentum Upgrade Activated!");
        }
    }

    // Call this when the player successfully dodges or evades
    public void OnDodge()
    {
        if (!isActive || !canTrigger || playerMovement == null) return;

        StartCoroutine(TriggerMomentum());
    }

    private IEnumerator TriggerMomentum()
    {
        canTrigger = false;

        float originalSpeed = playerMovement.maxSpeed;
        playerMovement.maxSpeed *= speedMultiplier;

        Debug.Log("Evasive Momentum Activated! Speed boosted.");

        yield return new WaitForSeconds(boostDuration);

        playerMovement.maxSpeed = originalSpeed;
        Debug.Log("Evasive Momentum ended.");

        yield return new WaitForSeconds(cooldown);
        canTrigger = true;
    }
}
