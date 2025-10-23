using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : AttackBase
{
    [Header("Melee Attack Settings")]
    [SerializeField] private float attackDuration = 0.15f;
    [SerializeField] private float rotationAngle = 45f;

    [Header("Slash Effect")]
    public GameObject slashEffectPrefab;
    private GameObject activeSlash;

    private Quaternion startRotation;
    private Quaternion attackRotation;

    // Input is handled by AttackController; this class only knows how to execute the attack
    protected override IEnumerator ExecuteAttack()
    {
        Console.WriteLine("SWINGING");

        // Spawn slash effect
        if (slashEffectPrefab != null)
        {
            activeSlash = Instantiate(
                slashEffectPrefab,
                transform.position + transform.forward * 0.7f,
                transform.rotation,
                transform   // parent to player so it moves with them
            );

            activeSlash.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Destroy(activeSlash, 1.0f);
        }

        // Simple rotation swing animation
        startRotation = transform.rotation;
        attackRotation = transform.rotation * Quaternion.Euler(0, rotationAngle, 0);

        float elapsed = 0f;
        while (elapsed < attackDuration)
        {
            elapsed += Time.deltaTime * 4f;
            transform.rotation = Quaternion.Slerp(startRotation, attackRotation, elapsed);
            yield return null;
        }

        // Reset rotation after swing
        transform.rotation = startRotation;

        // Attack completes this frame; cooldown is handled by base class
        yield break;
    }
}
