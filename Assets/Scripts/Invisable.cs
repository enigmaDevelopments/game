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
    public int defultLayer;
    public int intangableLayer;
    void Start()
    {
        if (user == null)
            user = transform.parent.gameObject;
    }

    protected override IEnumerator ExecuteAttack()
    {
        ThirdPersonMovement controller = user.GetComponent<ThirdPersonMovement>();
        Material material = user.GetComponent<Material>();
        if (intagable)
            user.layer = intangableLayer;
        controller.maxSpeed *= speedMultiplier;
        AI.playerInvisable = true;
        yield return new WaitForSeconds(duration);
        user.layer = defultLayer;
        controller.maxSpeed /= speedMultiplier;
        AI.playerInvisable = false;
        yield break;
    }



}
