using System.Collections;
using UnityEngine;

public class Laser : AttackBase
{
    [Header("Laser settings")]
    public LineRenderer lineRenderer;
    public float raduis;
    public float range;
    public LayerMask hitMask;

    private ulong timeStep = 0;
    private float lastTime = 0;


    protected override IEnumerator ExecuteAttack()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.endWidth = raduis * 2;
        Vector3[] positions = new Vector3[2];
        positions[0] = transform.position;
        if (Physics.SphereCast(transform.position, raduis, transform.forward, out RaycastHit hit, range, hitMask))
        {
            positions[1] = hit.point + hit.normal * raduis;
            Damage(hit.transform, damage * (lastTime==0?0:Time.time-lastTime));
            lastTime = Time.time;
        }
        else
        {
            positions[1] = transform.forward * range;
            lastTime = 0;
        }
        lineRenderer.SetPositions(positions);
        timeStep++;
        StartCoroutine(KillLaser(timeStep));
        yield break;
    }
    private IEnumerator KillLaser(ulong time)
    {
        yield return null;
        yield return null;
        yield return new WaitForEndOfFrame();
        if (time == timeStep)
        {
            lineRenderer.positionCount = 0;
            timeStep = 0;
            lastTime = 0;
        }
        yield break;
    }
}
