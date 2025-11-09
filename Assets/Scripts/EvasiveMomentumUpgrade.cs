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

    private PlayerMovement playerMovement;
    private bool canTrigger = true;

    void Start()
    {
        // Find the player and their movement script
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerMovement = player.GetComponent<PlayerMovement>();
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

        // Boost the player speed temporarily
        float originalSpeed = playerMovement.moveSpeed;
        playerMovement.moveSpeed *= speedMultiplier;

        Debug.Log("Evasive Momentum Activated! Speed boosted.");

        yield return new WaitForSeconds(boostDuration);

        // Restore original speed
        playerMovement.moveSpeed = originalSpeed;

        Debug.Log("Evasive Momentum ended.");

        // Start cooldown timer
        yield return new WaitForSeconds(cooldown);
        canTrigger = true;
    }
}
