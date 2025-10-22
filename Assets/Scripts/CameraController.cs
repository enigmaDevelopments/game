using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines;

public class CameraController : MonoBehaviour
{
    public LayerMask enviromentMask;
    public float upSpeed;
    public float downSpeed;
    public float standeredRaduis;
    public float minimumRaduis;
    public float detectionSteps = 1;
    private CinemachineOrbitalFollow follower;
    private CinemachineRotationComposer rotator;
    private Transform target;

    void Start()
    {
        follower = GetComponent<CinemachineOrbitalFollow>();
        target = GetComponent<CinemachineCamera>().Follow;
        rotator = GetComponent<CinemachineRotationComposer>();
    }

    void FixedUpdate()
    {
        follower.TrackerSettings.PositionDamping.y = transform.position.y - (Mathf.Tan(follower.VerticalAxis.Value * Mathf.Deg2Rad) * follower.Radius) <= target.position.y ? upSpeed : downSpeed;
    }
    private void Update()
    {
        Vector3 origin = transform.position + Quaternion.Euler(follower.VerticalAxis.Value, follower.HorizontalAxis.Value, 0) * Vector3.forward * follower.Radius;
        Vector3 direction = Quaternion.Euler(follower.VerticalAxis.Center, follower.HorizontalAxis.Value, 0) * Vector3.back;
        Debug.DrawLine(target.position, transform.position, Color.red);
        Debug.DrawRay(origin, direction * standeredRaduis, Color.blue);
        if (Physics.Linecast(target.position,transform.position) || Physics.Raycast(origin,direction, standeredRaduis))
        {
            float bestAngle = -1;
            float bestDistance = float.NegativeInfinity;

            for (float i = follower.VerticalAxis.Range.x; i < follower.VerticalAxis.Range.y; i += detectionSteps)
            {
                Vector3 newPosition = origin + Quaternion.Euler(i, follower.HorizontalAxis.Value, 0) * Vector3.back * standeredRaduis;
                Debug.DrawLine(target.position, newPosition, Color.green);
                if (Physics.Linecast(target.position,newPosition,out RaycastHit hit,enviromentMask))
                {
                    if (bestDistance < hit.distance)
                    {
                        if (standeredRaduis <= hit.distance)
                        {
                            bestDistance = minimumRaduis;
                            bestAngle = i;
                            break;
                        }
                        bestDistance = hit.distance;
                        bestAngle = i;
                    }
                }
                else
                {
                    bestDistance = standeredRaduis;
                    bestAngle = i;
                    break;
                }
            }
            if (minimumRaduis < bestDistance)
            {
                follower.Radius = bestDistance;
                follower.VerticalAxis.Value = bestAngle;
            }
        }
        else
        {
            follower.Radius = standeredRaduis;
            follower.VerticalAxis.Value = follower.VerticalAxis.Center;
        }
        
    }
}
