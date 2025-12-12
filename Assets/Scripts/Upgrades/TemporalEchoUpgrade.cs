//using System.Collections;
//using UnityEngine;

//public class TemporalEchoUpgrade : MonoBehaviour
//{
//    [Header("Temporal Echo Settings")]
//    public bool isActive = false;           // If the upgrade is active
//    public float echoDelay = 1f;            // Time delay for the echo to repeat
//    public float echoDuration = 2f;         // Duration the echo lasts
//    public float echoRepeatFactor = 1f;     // How much the echo duplicates the player's movement speed

//    private PlayerMovement playerMovement;
//    private PlayerAttack playerAttack;

//    private void Start()
//    {
//        playerMovement = FindAnyObjectByType<PlayerMovement>();
//        playerAttack = FindAnyObjectByType<PlayerAttack>();
//    }

//    public void Activate()
//    {
//        if (!isActive)
//        {
//            isActive = true;
//            Debug.Log("? Temporal Echo Upgrade Activated!");
//        }
//    }

//    public void TriggerEcho()
//    {
//        if (isActive)
//        {
//            // Start the temporal echo process
//            StartCoroutine(SpawnTemporalEcho());
//        }
//    }

//    private IEnumerator SpawnTemporalEcho()
//    {
//        // Create a duplicate of the player's position
//        Vector2 originalPosition = playerMovement.transform.position;
//        Vector2 echoPosition = originalPosition;

//        // Delay before the echo happens
//        yield return new WaitForSeconds(echoDelay);

//        // Move the echo in the same direction as the player
//        playerMovement.transform.position = echoPosition;

//        // Duplicate movement or actions based on your needs
//        // For example, if the player is moving, duplicate that
//        if (playerMovement != null)
//        {
//            Vector2 movement = playerMovement.GetMovementDirection() * echoRepeatFactor;
//            playerMovement.transform.Translate(movement);
//        }

//        // Optional: duplicate attack action (e.g., re-trigger attack)
//        if (playerAttack != null)
//        {
//            playerAttack.RepeatAttack();  // You'll need to define this method
//        }

//        // Keep the echo active for a short duration
//        yield return new WaitForSeconds(echoDuration);

//        // Optional: Reset the player's position back to the original state
//        playerMovement.transform.position = originalPosition;
//    }

//}
using System.Collections;
using UnityEngine;

public class TemporalEchoUpgrade : MonoBehaviour
{
    [Header("Temporal Echo Settings")]
    public bool isActive = false;           // If the upgrade is active
    public float echoDelay = 1f;            // Time delay for the echo to repeat
    public float echoDuration = 2f;         // Duration the echo lasts
    public float echoRepeatFactor = 1f;     // How much the echo duplicates the player's movement speed

    private ThirdPersonMovement playerMovement;
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
        if (isActive)
            StartCoroutine(SpawnTemporalEcho());
    }

    private IEnumerator SpawnTemporalEcho()
    {
        if (playerMovement == null) yield break;

        // Create a duplicate of the player's position (use Vector3 for 3D position)
        Vector3 originalPosition = playerMovement.transform.position;
        Vector3 echoPosition = originalPosition;

        // Delay before the echo happens
        yield return new WaitForSeconds(echoDelay);

        // Move the echo to the starting position
        playerMovement.transform.position = echoPosition;

        // Move in the same direction
        Vector3 movement = playerMovement.currentMoveDirection * echoRepeatFactor;
        playerMovement.transform.Translate(movement, Space.World);

        // Repeat attack if available
        /*if (playerAttack != null)
            playerAttack.RepeatAttack();*/

        yield return new WaitForSeconds(echoDuration);

        playerMovement.transform.position = originalPosition;
    }
}
