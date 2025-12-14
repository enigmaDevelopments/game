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
        public AngleData() { }
    }

    void Start()
    {
        camera = GetComponent<CinemachineCamera>();
        follower = GetComponent<CinemachineOrbitalFollow>();

        if (camera == null || follower == null)
        {
            Debug.LogError("CameraController: Missing CinemachineCamera or CinemachineOrbitalFollow component.");
            enabled = false;
            return;
        }

        // If the CinemachineCamera already has a follow target set in the Inspector, use it
        if (camera.Follow != null)
        {
            target = camera.Follow;
        }
    }

    // ? Allow other scripts (like PlayerSpawner) to assign the player
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        if (camera != null)
        {
            camera.Follow = newTarget;
        }
    }

    private void Update()
    {
        // ? No target yet? Do nothing this frame (prevents NullReferenceException)
        if (target == null || follower == null || camera == null)
        {
            return;
        }

        #region change camera speed if player is jumping
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
        #endregion

        #region check if camra can see player
        distent = 10 < Vector3.Distance(origin, target.position);
        if (!distent)
            origin = target.position;
        Vector3 position = origin + Quaternion.Euler(follower.VerticalAxis.Center, follower.HorizontalAxis.Value, 0) * Vector3.back * follower.Radius;
        Vector3 direction = (target.position - position).normalized;
        float distance = Mathf.Max(Vector3.Distance(position, target.position), follower.Radius + 1);
        #endregion

        if (Physics.Raycast(position, direction, distance, enviromentMask))
        {
            #region move camera vertically axis and zoom if posible
            AngleData best = BestAngle(follower.HorizontalAxis.Value, detectionSteps);
            if (minimumRaduis < best.distance && !distent)
            {
                follower.RadialAxis.Value = best.distance / follower.Radius;
                follower.VerticalAxis.Value = best.angle;
            }
            #endregion

            #region keep camera in radius by moving the camera horizontally
            else
            {
                float startAngle = follower.HorizontalAxis.Value;
                bool found = false;
                for (float i = 0; i < 180; i += detectionStepsHorizontal)
                {
                    float angle = ((startAngle + i + 180) % 360 + 360) % 360 - 180;
                    AngleData sideBest = BestAngle(angle, detectionStepsInternal);
                    if ((distent && sideBest.noHit) || (!distent && minimumRaduis < sideBest.distance))
                    {
                        found = true;
                        follower.RadialAxis.Value = sideBest.distance / follower.Radius;
                        follower.VerticalAxis.Value = sideBest.angle;
                        follower.HorizontalAxis.Value = angle;
                        break;
                    }

                    angle = ((startAngle - i + 180) % 360 + 360) % 360 - 180;
                    sideBest = BestAngle(angle, detectionStepsInternal);
                    if ((distent && sideBest.noHit) || (!distent && minimumRaduis < sideBest.distance))
                    {
                        found = true;
                        follower.RadialAxis.Value = sideBest.distance / follower.Radius;
                        follower.VerticalAxis.Value = sideBest.angle;
                        follower.HorizontalAxis.Value = angle;
                        break;
                    }
                }
                #endregion

                #region if cant find a position keep the best found
                if (!found)
                {
                    follower.RadialAxis.Value = best.distance / follower.Radius;
                    follower.VerticalAxis.Value = best.angle;
                }
                #endregion
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
            if (target != null)
                Debug.DrawLine(target.position, newPosition, Color.green);
#endif
            if (target != null && Physics.Linecast(target.position, newPosition, out RaycastHit hit, enviromentMask))
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
