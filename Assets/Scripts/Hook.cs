using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Hook : Projectile
{
    public float pullInTime = 2;
    private Transform parent;
    private Vector3 startingPos;
    private Vector3 endingPos;
    private Quaternion startingRoation;
    private Quaternion endingRoation;
    private bool hooked = false;
    private bool returning = false;
    private float timer = 0;
    private ThirdPersonMovement controller;


    protected override void Start()
    {
        StartCoroutine(ReturnProjectile());
        parent = owner.transform.parent;
        controller = parent.GetComponent<ThirdPersonMovement>();
    }
    private void FixedUpdate()
    {
        if (hooked)
        {
            parent.position = Vector3.Lerp(startingPos, endingPos, timer);
            parent.rotation = Quaternion.Slerp(startingRoation, endingRoation, timer);
            timer += Time.deltaTime / pullInTime;
        }
        else if (returning)
        {
            transform.position = Vector3.Lerp(startingPos, owner.transform.position, timer);
            timer += Time.deltaTime / pullInTime;
        }
        if (1 < timer)
        {
            controller.enabled = true;
            Destroy(gameObject);
        }

    }

    protected override void OnCollisionEnter(Collision collision)
    {
        hooked = true;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        startingPos = parent.position;
        endingPos = transform.position -(parent.rotation* Vector3.Scale(parent.lossyScale, owner.transform.localPosition));
        startingRoation = parent.rotation;
        endingRoation = Quaternion.LookRotation(owner.transform.position - transform.position, Vector3.up);
        controller.enabled = false;
    }
    private IEnumerator ReturnProjectile()
    {
        yield return new WaitForSeconds(duration);
        if (!hooked)
        {
            returning = true;
            startingPos = transform.position;
        }
        yield break;
    }
}