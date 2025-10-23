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
    public float standeredRaduis;
    public float minimumRaduis;
    public float detectionSteps = 1;
    public float detectionStepsHorizontal = 5;
    private CinemachineOrbitalFollow follower;
    private CinemachineRotationComposer rotator;
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
        rotator = GetComponent<CinemachineRotationComposer>();
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

        Vector3 direction = Quaternion.Euler(follower.VerticalAxis.Center, follower.HorizontalAxis.Value, 0) * Vector3.back;
        Debug.DrawLine(target.position, transform.position, Color.red);
        Debug.DrawRay(origin, direction * standeredRaduis, Color.blue);
        if (Physics.Linecast(target.position,transform.position) || Physics.Raycast(origin,direction, standeredRaduis))
        {
            AngleData best = BestAngle(follower.HorizontalAxis.Value);
            if (minimumRaduis < best.distance)
            {
                follower.Radius = best.distance;
                follower.VerticalAxis.Value = best.angle;
            }
            else
            {
                float startAngle = follower.HorizontalAxis.Value;
                Debug.Log(startAngle);
                for (float i = 0; i < 180; i += detectionStepsHorizontal)
                {
                    float angle = ((startAngle + i + 180) % 360 + 360) % 360 - 180;
                    AngleData sideBest = BestAngle(angle);
                    if (minimumRaduis < sideBest.distance)
                    {
                        follower.Radius = sideBest.distance;
                        follower.VerticalAxis.Value = sideBest.angle;
                        follower.HorizontalAxis.Value = angle;
                        return;
                    }
                    angle = ((startAngle - i + 180) % 360 + 360) % 360 - 180;
                    sideBest = BestAngle(angle);
                    if (minimumRaduis < sideBest.distance)
                    {
                        follower.Radius = sideBest.distance;
                        follower.VerticalAxis.Value = sideBest.angle;
                        follower.HorizontalAxis.Value = angle;
                        return;
                    }
                }
            }
        }
        else
        {
            follower.Radius = standeredRaduis;
            follower.VerticalAxis.Value = follower.VerticalAxis.Center;
        }
    }

    private AngleData BestAngle(float position)
    {
        AngleData best = new AngleData();

        for (float i = follower.VerticalAxis.Range.x; i < follower.VerticalAxis.Range.y; i += detectionSteps)
        {
            Vector3 newPosition = origin + Quaternion.Euler(i, position, 0) * Vector3.back * standeredRaduis;
            Debug.DrawLine(target.position, newPosition, Color.green);
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
                best.distance = standeredRaduis;
                best.angle = i;
                return best;
            }
        }
        return best;
    }
}
