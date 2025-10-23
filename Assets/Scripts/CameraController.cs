using System.Xml;
using Unity.Cinemachine;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public LayerMask enviromentMask;
    public float jumpSpeed;
    public float middleSpeed;
    public float normalSpeed;
    public float maxJumpHeight;
    public float maxJumpTime;
    public float minimumRaduis;
    public float detectionSteps = 1;
    public float detectionStepsHorizontal = 5;
    public float detectionStepsInternal = 1;
    public float defultLensFOV;
    private new CinemachineCamera camera;
    private CinemachineOrbitalFollow follower;
    private Transform target;
    private Vector3 origin;
    private float jumpTimer = 0;
    private bool distent;

    private class AngleData
    {
        public float angle = float.NegativeInfinity;
        public float distance = float.NegativeInfinity;
        public bool noHit = false;
        public AngleData(){}
    }


    void Start()
    {
        camera = GetComponent<CinemachineCamera>();
        follower = GetComponent<CinemachineOrbitalFollow>();
        target = GetComponent<CinemachineCamera>().Follow;
    }
    private void Update()
    {
        origin = transform.position + Quaternion.Euler(follower.VerticalAxis.Value, follower.HorizontalAxis.Value, 0) * Vector3.forward * follower.Radius;
        if (maxJumpHeight < target.position.y - origin.y)
        {
            follower.TrackerSettings.PositionDamping.y = normalSpeed;
            jumpTimer = float.PositiveInfinity;
        }
        else
        {
            if (origin.y + 1 <= target.position.y)
            {
                if (jumpTimer < maxJumpTime)
                {
                    follower.TrackerSettings.PositionDamping.y = jumpSpeed;
                    jumpTimer += Time.deltaTime;
                }
                else
                    follower.TrackerSettings.PositionDamping.y = middleSpeed;
            }
            else
            {
                follower.TrackerSettings.PositionDamping.y = normalSpeed;
                jumpTimer = 0;
            }
        }
        distent = 10 < Vector3.Distance(origin, target.position);
        if (!distent)
            origin = target.position;
        Vector3 position = origin + Quaternion.Euler(follower.VerticalAxis.Center, follower.HorizontalAxis.Value, 0) * Vector3.back * follower.Radius;
        Vector3 direction = (target.position - position).normalized;
        float distance = Mathf.Max(Vector3.Distance(position,target.position), follower.Radius + 1);

        if (Physics.Raycast(position, direction, distance, enviromentMask))
        {
            AngleData best = BestAngle(follower.HorizontalAxis.Value, detectionSteps);
            if (minimumRaduis < best.distance && !distent)
            {
                follower.RadialAxis.Value = best.distance/follower.Radius;
                follower.VerticalAxis.Value = best.angle;
            }
            else
            {
                float startAngle = follower.HorizontalAxis.Value;
                for (float i = 0; i < 180; i += detectionStepsHorizontal)
                {
                    float angle = ((startAngle + i + 180) % 360 + 360) % 360 - 180;
                    AngleData sideBest = BestAngle(angle, detectionStepsInternal);
                    if ((distent && sideBest.noHit) || (!distent && minimumRaduis < sideBest.distance))
                    {
                        follower.RadialAxis.Value = sideBest.distance / follower.Radius;
                        follower.VerticalAxis.Value = sideBest.angle;
                        follower.HorizontalAxis.Value = angle;
                        break;
                    }

                    angle = ((startAngle - i + 180) % 360 + 360) % 360 - 180;
                    sideBest = BestAngle(angle, detectionStepsInternal);
                    if ((distent && sideBest.noHit) || (!distent && minimumRaduis < sideBest.distance))
                    {
                        follower.RadialAxis.Value = sideBest.distance / follower.Radius;
                        follower.VerticalAxis.Value = sideBest.angle;
                        follower.HorizontalAxis.Value = angle;
                        break;
                    }
                }
            }
        }
        else
        {
            follower.RadialAxis.Value = 1;
            follower.VerticalAxis.Value = follower.VerticalAxis.Center;
        }
        camera.Lens.FieldOfView = defultLensFOV / follower.RadialAxis.Value;
    }

    private AngleData BestAngle(float position, float steps)
    {
        AngleData best = new AngleData();

        for (float i = follower.VerticalAxis.Range.x; i < follower.VerticalAxis.Range.y; i += steps)
        {
            Vector3 newPosition = origin + Quaternion.Euler(i, position, 0) * Vector3.back * follower.Radius;
            #if UNITY_EDITOR
                Debug.DrawLine(target.position, newPosition, Color.green);
            #endif
            if (Physics.Linecast(target.position, newPosition, out RaycastHit hit, enviromentMask))
            {
                if (best.distance < hit.distance)
                {
                    best.distance = hit.distance;
                    best.angle = i;
                }
            }
            else
            {
                best.noHit = true;
                best.distance = follower.Radius;
                best.angle = i;
                return best;
            }
        }
        return best;
    }
}
