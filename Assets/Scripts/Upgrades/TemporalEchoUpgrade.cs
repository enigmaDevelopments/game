using System.Collections;
using UnityEngine;

public class TemporalEchoUpgrade : MonoBehaviour
{
    [Header("Temporal Echo Settings")]
    public bool isActive = false;           // If the upgrade is active
    public float echoDelay = 1f;            // Time delay for the echo to repeat
    public float echoDuration = 2f;         // Duration the echo lasts
    public float echoRepeatFactor = 1f;     // How much the echo duplicates the player's movement speed

    public ThirdPersonMovement playerMovement;
    private AttackBase playerAttack;

    private void Start()
    {
        // Optional: find the player on start
        if (playerMovement == null)
            playerMovement = FindAnyObjectByType<ThirdPersonMovement>();

        if (playerAttack == null)
            playerAttack = FindAnyObjectByType<AttackBase>();
    }

    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;

            if (playerMovement == null)
                playerMovement = FindAnyObjectByType<ThirdPersonMovement>();

            if (playerAttack == null)
                playerAttack = FindAnyObjectByType<AttackBase>();

            Debug.Log("Temporal Echo Upgrade Activated!");
        }
    }

    public void TriggerEcho()
    {
        if (isActive && playerMovement != null)
        {
            // ?? Grab direction at the time you trigger the echo
            Vector3 dir = playerMovement.CurrentMoveDirection;
            StartCoroutine(SpawnTemporalEcho(dir));
        }
    }

    private IEnumerator SpawnTemporalEcho(Vector3 echoDirection)
    {
        if (playerMovement == null) yield break;

        // Create a duplicate of the player's position (use Vector3 for 3D position)
        Vector3 originalPosition = playerMovement.transform.position;
        Vector3 echoPosition = originalPosition;

        // Delay before the echo happens
        yield return new WaitForSeconds(echoDelay);

        // Move the echo to the starting position
        playerMovement.transform.position = echoPosition;

        // Move in the same direction that was cached
        Vector3 movement = echoDirection * echoRepeatFactor;
        playerMovement.transform.Translate(movement, Space.World);

        // Repeat attack if available
        /*if (playerAttack != null)
            playerAttack.RepeatAttack();*/

        yield return new WaitForSeconds(echoDuration);

        playerMovement.transform.position = originalPosition;
    }
}
