using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Laser : AttackBase
{
    public LineRenderer lineRenderer;
    public float raduis;

    private void Start()
    {
        lineRenderer.endWidth = raduis;
    }

    protected override IEnumerator ExecuteAttack()
    {

        if (Physics.SphereCast(transform.position, raduis, transform.forward, out RaycastHit hit))
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(new Vector3[] { transform.position, hit.point + hit.normal * raduis });
        }
        else
            lineRenderer.positionCount = 0;
        yield break;
    }
}
