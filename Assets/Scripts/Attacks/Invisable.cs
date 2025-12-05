using System.Collections;
using UnityEngine;

public class Invisable : AttackBase
{
    [Tooltip("Optional. If not provided, will use parent")]
    public GameObject user;
    [Header("Invisable Settings")]
    public float duration;
    public float speedMultiplier = 1f;
    public bool intagable = false;
    private IntangibilityManager IntangibilityManager;
    void Start()
    {
        if (user == null)
            user = transform.parent.gameObject;
        IntangibilityManager = user.GetComponent<IntangibilityManager>();
    }

    protected override IEnumerator ExecuteAttack()
    {
        ThirdPersonMovement controller = user.GetComponent<ThirdPersonMovement>();
        Material material = user.GetComponent<Material>();
        if (intagable)
            IntangibilityManager.Timer = duration;
        controller.maxSpeed *= speedMultiplier;
        AI.playerInvisable = true;
        yield return new WaitForSeconds(duration);
        controller.maxSpeed /= speedMultiplier;
        AI.playerInvisable = false;
        yield break;
    }



}
