using System.Collections;
using UnityEngine;

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
    private LineRenderer lineRenderer;
    private CharacterController characterController;


    protected override void awake()
    {
        StartCoroutine(ReturnProjectile());
        parent = owner.transform.parent;
        controller = parent.GetComponent<ThirdPersonMovement>();
        lineRenderer = owner.GetComponent<LineRenderer>();
        characterController = parent.GetComponent<CharacterController>();
        lineRenderer.enabled = true;
    }
    private void FixedUpdate()
    {
        lineRenderer.SetPositions(new Vector3[] {owner.transform.position, transform.position});
        parent.rotation = Quaternion.LookRotation(transform.position - parent.transform.position, parent.transform.up);
        if (hooked)
        {
            characterController.Move(Vector3.Lerp(startingPos, endingPos, timer)-parent.transform.position);
            parent.rotation = Quaternion.Slerp(startingRoation, endingRoation, timer * 3);
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
            lineRenderer.enabled = false;
            Destroy(gameObject);
        }
        if (!hooked)
        {
            Vector3 direction = transform.position - parent.transform.position;
            direction.y = 0;
            parent.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

    }

    protected override void OnCollisionEnter(Collision collision)
    {
        hooked = true;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        startingPos = parent.position;
        endingPos = transform.position -(parent.rotation* Vector3.Scale(parent.lossyScale, owner.transform.localPosition));
        startingRoation = parent.rotation;
        endingRoation = Quaternion.LookRotation(transform.position - owner.transform.position, Vector3.up);
        controller.enabled = false;
    }
    private IEnumerator ReturnProjectile()
    {
        yield return new WaitForSeconds(duration);
        if (!hooked)
        {
            returning = true;
            startingPos = transform.position;
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
        yield break;
    }
}