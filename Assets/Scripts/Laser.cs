using System.Collections;
using UnityEngine;

public class Laser : AttackBase
{
    [Header("Laser settings")]
    public LineRenderer lineRenderer;
    public float raduis;

    private ulong timeStep = 0;


    protected override IEnumerator ExecuteAttack()
    {

        if (Physics.SphereCast(transform.position, raduis, transform.forward, out RaycastHit hit))
        {
            lineRenderer.positionCount = 2;
            lineRenderer.endWidth = raduis*2;
            lineRenderer.SetPositions(new Vector3[] { transform.position, hit.point + hit.normal * raduis });
            timeStep++;
            StartCoroutine(KillLaser(timeStep));
        }
        else
            lineRenderer.positionCount = 0;
        yield break;
    }
    private IEnumerator KillLaser(ulong time)
    {
        yield return null;
        yield return new WaitForEndOfFrame();
        if (time == timeStep)
        {
            lineRenderer.positionCount = 0;
            timeStep = 0;
        }
        yield break;
    }
}
