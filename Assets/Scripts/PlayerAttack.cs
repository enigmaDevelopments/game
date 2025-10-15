using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private PlayerInput controls;
    private bool isAttacking = false;
    private bool canAttack = true;

    public float attackDuration = 0.15f; 
    public float attackCooldown = 0.5f; 
    public float rotationAngle = 45f;    

    [Header("Slash Effect")]
    public GameObject slashEffectPrefab; 
    private GameObject activeSlash;      

    private Quaternion startRotation;
    private Quaternion attackRotation;

    void Start()
    {
        controls = GetComponent<PlayerInput>();
        Console.WriteLine("PlayerAttack initialized.");
    }

    void Update()
    {
        // Only allow attacking if not already swinging and cooldown is over
        if (controls.actions["Attack"].WasPerformedThisFrame() && !isAttacking && canAttack)
        {
            StartCoroutine(Swing());
        }
    }

    private System.Collections.IEnumerator Swing()
    {
        Console.WriteLine("SWINGING");

        canAttack = false;
        isAttacking = true;

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

        transform.rotation = startRotation;
        isAttacking = false;

        // Wait for cooldown after attack finishes
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        Console.WriteLine("Cooldown finished. Ready to attack again.");
    }
}
