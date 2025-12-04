using UnityEngine;
using System.Collections;

public class MomentumUpgrade : MonoBehaviour
{
    [Header("Momentum Settings")]
    [Tooltip("Enable or disable the upgrade")]
    public bool isActive = false;

    [Tooltip("How much speed is added per second of movement")]
    public float speedGainRate = 0.1f;

    [Tooltip("How much damage bonus is added per second of movement")]
    public float damageGainRate = 0.05f;

    [Tooltip("Maximum movement speed multiplier")]
    public float maxSpeedMultiplier = 1.5f;

    [Tooltip("Maximum damage multiplier")]
    public float maxDamageMultiplier = 1.5f;

    [Tooltip("How quickly momentum decays when idle or hit")]
    public float decayRate = 1f;

    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;
    private float currentSpeedBonus = 1f;
    private float currentDamageBonus = 1f;
    private bool isMoving = false;

    private void Start()
    {
        // Find player components
        GameObject player = GameObject.FindGameObjectWithTag("Player");  // update with correct tag
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            playerCombat = player.GetComponent<PlayerCombat>();
        }

        // Subscribe to damage event if you have one
        PlayerHealth.OnPlayerDamaged += ResetMomentum;
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerDamaged -= ResetMomentum;
    }

    private void Update()
    {
        if (!isActive || playerMovement == null) return;

        // Check if the player is moving
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        isMoving = input.sqrMagnitude > 0.1f;

        if (isMoving)
        {
            // Build momentum over time
            currentSpeedBonus += speedGainRate * Time.deltaTime;
            currentDamageBonus += damageGainRate * Time.deltaTime;
        }
        else
        {
            // Decay momentum when idle
            currentSpeedBonus -= decayRate * Time.deltaTime;
            currentDamageBonus -= decayRate * Time.deltaTime;
        }

        // Clamp the bonuses
        currentSpeedBonus = Mathf.Clamp(currentSpeedBonus, 1f, maxSpeedMultiplier);
        currentDamageBonus = Mathf.Clamp(currentDamageBonus, 1f, maxDamageMultiplier);

        // Apply to player systems
        playerMovement.momentumMultiplier = currentSpeedBonus;
        if (playerCombat != null)
            playerCombat.damageMultiplier = currentDamageBonus;
    }

    private void ResetMomentum()
    {
        if (!isActive) return;

        Debug.Log("Momentum Lost!");
        currentSpeedBonus = 1f;
        currentDamageBonus = 1f;
    }
}
