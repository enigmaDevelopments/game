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
    private IntangibilityManager intangibilityManager;
    void Start()
    {
        if (user == null)
            user = transform.root.gameObject;
        intangibilityManager = user.GetComponent<IntangibilityManager>();
    }

    protected override IEnumerator ExecuteAttack()
    {
        Debug.Log("invisable");
        ThirdPersonMovement controller = user.GetComponent<ThirdPersonMovement>();
        if (intagable)
        {
            intangibilityManager.Timer = duration;
            intangibilityManager.flashType = IntangibilityManager.FlashType.invisable;
        }
        controller.maxSpeed *= speedMultiplier;
        AI.playerInvisable = true;
        yield return new WaitForSeconds(duration);
        controller.maxSpeed /= speedMultiplier;
        AI.playerInvisable = false;
        yield break;
    }



}
