using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineOrbitalFollow follower;
    public float upSpeed;
    public float downSpeed;

    void FixedUpdate()
    {
        follower.TrackerSettings.PositionDamping.y = follower.transform.position.y < (transform.position.y + 3) ? upSpeed : downSpeed;
    }
}
