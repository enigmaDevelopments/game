using System.Collections;
using UnityEngine;

public class BubbleGenerater : AttackBase
{

    public float diamater;
    public float duration;
    public float speed;
    [Header("Reference")]
    public GameObject bubble;

    private Transform bubbleInstence;

    private void FixedUpdate()
    {
        if (isAttacking)
            if (bubbleInstence.localScale.x < diamater)
                bubbleInstence.localScale += Vector3.one * speed * Time.deltaTime;
    }
    protected override IEnumerator ExecuteAttack()
    {
        bubbleInstence = Instantiate(bubble,transform).transform;
        yield return new WaitForSeconds(duration);
        Destroy(bubbleInstence.gameObject);
        yield break;
    }
}
