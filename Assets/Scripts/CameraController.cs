using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float upSpeed;
    public float downSpeed;
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
}
