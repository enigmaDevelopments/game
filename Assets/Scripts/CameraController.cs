using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public LayerMask enviromentMask;
    public float upSpeed;
    public float downSpeed;
    public float standeredRaduis;
    public float middleRaduis;
    public float closestRaduis;
    private CinemachineOrbitalFollow follower;
    private Transform target;

    void Start()
    {
        follower = GetComponent<CinemachineOrbitalFollow>();
        target = GetComponent<CinemachineCamera>().Follow;
    }

    void FixedUpdate()
    {
        follower.TrackerSettings.PositionDamping.y = transform.position.y - (Mathf.Tan(follower.VerticalAxis.Value * Mathf.Deg2Rad) * follower.Radius) <= target.position.y ? upSpeed : downSpeed;
    }
    private void Update()
    {
        if (Physics.Linecast(target.position, transform.position, out RaycastHit hit, enviromentMask))
        {
            if (middleRaduis < hit.distance)
                follower.Radius = hit.distance;
        }
        else
            follower.Radius = standeredRaduis;
    }
}
