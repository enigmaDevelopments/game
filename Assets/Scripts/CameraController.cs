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
    private CinemachineOrbitalFollow follower;
    private Transform target;
    private Vector3 origin;
    private float jumpTimer = 0;

    private class AngleData
    {
        public float angle = float.NegativeInfinity;
        public float distance = float.NegativeInfinity;
        public AngleData(){}
    }


    void Start()
    {
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
            origin = target.position;
        }   

        Vector3 direction = (transform.position-origin).normalized;
        float distance = Mathf.Max(Vector3.Distance(origin, transform.position), follower.Radius + 1);
        if (Physics.Raycast(origin, direction, distance, enviromentMask))
        {
            AngleData best = BestAngle(follower.HorizontalAxis.Value);
            if (minimumRaduis < best.distance)
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
                    AngleData sideBest = BestAngle(angle);
                    if (minimumRaduis < sideBest.distance)
                    {
                        follower.RadialAxis.Value = sideBest.distance/follower.Radius;
                        follower.VerticalAxis.Value = sideBest.angle;
                        follower.HorizontalAxis.Value = angle;
                        break;
                    }
                    angle = ((startAngle - i + 180) % 360 + 360) % 360 - 180;
                    sideBest = BestAngle(angle);
                    if (minimumRaduis < sideBest.distance)
                    {
                        follower.RadialAxis.Value = sideBest.distance/follower.Radius;
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
    }

    private AngleData BestAngle(float position)
    {
        AngleData best = new AngleData();

        for (float i = follower.VerticalAxis.Range.x; i < follower.VerticalAxis.Range.y; i += detectionSteps)
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
                best.distance = follower.Radius;
                best.angle = i;
                return best;
            }
        }
        return best;
    }
}
