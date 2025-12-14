using Unity.Cinemachine;
using UnityEngine;

public class CameraData : MonoBehaviour
{
    public CinemachineCamera normalCamera;
    public CinemachineCamera aimCamera;
    public Canvas crosshairCanvas;

    private void Start()
    {
        normalCamera.Target = new CameraTarget { TrackingTarget = GameObject.FindGameObjectWithTag("Player").transform };
        aimCamera.Target = new CameraTarget { TrackingTarget = GameObject.FindGameObjectWithTag("Aim").transform };
    }
}
