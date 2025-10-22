using Unity.Cinemachine;
using UnityEngine;
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
        float distance = Mathf.Max(Vector3.Distance(transform.position, target.position), standeredRaduis);
        Vector3 direction = (transform.position - target.position).normalized;

        if (Physics.Raycast(new Ray(target.position,direction),out RaycastHit hit, distance, enviromentMask))
        {
            if (minimumRaduis < hit.distance)
                follower.Radius = hit.distance;
            //else
            //{
                
            //}
        }
        else
            follower.Radius = standeredRaduis;


        Vector3 origin = transform.position + Quaternion.Euler(follower.VerticalAxis.Value, follower.HorizontalAxis.Value, 0) * Vector3.forward * standeredRaduis;
        Debug.DrawLine(transform.position, origin, Color.red);
        for (float i = follower.VerticalAxis.Range.x; i < follower.VerticalAxis.Range.y; i += detectionSteps)
        {
            Vector3 newPosition = origin + Quaternion.Euler(i, follower.HorizontalAxis.Value, 0) * Vector3.back * standeredRaduis;
            Vector3 newDirection = (target.position - newPosition).normalized;

            Debug.DrawRay(newPosition, newDirection * distance, Color.green);
            Ray ray = new Ray(newPosition, newDirection);

        }
    }
}
