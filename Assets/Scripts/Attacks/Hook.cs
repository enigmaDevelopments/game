using System.Collections;
using UnityEngine;

public class Hook : Projectile
{
    public float pullInSpeed = 2;
    public Vector3 rotationOffset;

    private Transform grandParent;
    private Renderer parentRenderer;
    private Transform root;
    private Vector3 startingPos;
    private Vector3 endingPos;
    private Quaternion startingRoation;
    private bool hooked = false;
    private bool returning = false;
    private float timer = 0;
    private float pullInTime = 0;
    private ThirdPersonMovement controller;
    private LineRenderer lineRenderer;
    private CharacterController characterController;


    protected override void Start()
    {
        StartCoroutine(ReturnProjectile());
        Transform parent = owner.transform.parent;
        grandParent = parent.parent.parent.parent;
        root = owner.transform.root;
        startingRoation = grandParent.rotation;
        controller = root.GetComponent<ThirdPersonMovement>();
        lineRenderer = owner.GetComponent<LineRenderer>();
        characterController = root.GetComponent<CharacterController>();
        lineRenderer.enabled = true;
        GetComponent<MeshFilter>().mesh = parent.GetComponent<MeshFilter>().mesh;
        parentRenderer = parent.GetComponent<Renderer>();
        GetComponent<Renderer>().materials = parentRenderer.materials;
        transform.localScale = parent.lossyScale;
        transform.rotation = parent.rotation;
        parentRenderer.enabled = false;
    }
    private void Update()
    {
        if (1 < timer)
        {
            controller.enabled = true;
            lineRenderer.enabled = false;
            grandParent.rotation = startingRoation;
            parentRenderer.enabled = true;
            Destroy(gameObject, .1f);
            return;
        }
        Vector3 directionY = (transform.position - owner.transform.position).normalized;
        Vector3 direction = directionY;
        direction.y = 0;
        direction.Normalize();
        root.rotation = Quaternion.LookRotation(direction);
        grandParent.rotation = Quaternion.LookRotation(directionY) * Quaternion.Inverse(Quaternion.Euler(rotationOffset));
        if (hooked)
        {
            characterController.Move(Vector3.Lerp(startingPos, endingPos, timer)-root.transform.position);
            timer += Time.deltaTime / pullInTime;
        }
        else if (returning)
        {
            transform.position = Vector3.Lerp(startingPos, owner.transform.position, timer);
            timer += Time.deltaTime / pullInTime;
        }
        lineRenderer.SetPositions(new Vector3[] { owner.transform.position, transform.position });
    }

    protected override void OnTriggerEnter(Collider collision)
    {
        hooked = true;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        startingPos = owner.transform.position;
        endingPos = transform.position + root.position - owner.transform.position;
        controller.enabled = false;
        pullInTime = Vector3.Distance(transform.position, owner.transform.position) / pullInSpeed;
    }
    private IEnumerator ReturnProjectile()
    {
        yield return new WaitForSeconds(duration);
        if (!hooked)
        {
            returning = true;
            startingPos = transform.position;
            pullInTime = Vector3.Distance(transform.position, owner.transform.position)/pullInSpeed;
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
        yield break;
    }
}