using UnityEngine;

public class Cooldown : MonoBehaviour
{
    public float cooldownDuration = 3f;
    private float cooldownTimer = 0f;
    private bool isOnCooldown = false;

    public InGameMenu inGameMenu; // assign in Inspector

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) // Dash or Blast key
        {
            TryUseAbility();
        }

        // If we’re cooling down, tick timer and update UI
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            float progress = 1f - (cooldownTimer / cooldownDuration);
            inGameMenu.UpdateCooldownBar(progress);

            if (cooldownTimer <= 0f)
            {
                cooldownTimer = 0f;
                isOnCooldown = false;
                inGameMenu.UpdateCooldownBar(1f); // Full again
            }
        }
    }

    void TryUseAbility()
    {
        if (!isOnCooldown)
        {
            Debug.Log("Ability used!");
            ActivateAbility();

            isOnCooldown = true;
            cooldownTimer = cooldownDuration;
            inGameMenu.UpdateCooldownBar(0f);
        }
    }

    void ActivateAbility()
    {
        // Put dash/blast logic here
        // or whatever is going to use cooldown.
    }
}