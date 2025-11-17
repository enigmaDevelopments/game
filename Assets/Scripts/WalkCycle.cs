using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class WalkCycle : MonoBehaviour
{
    [Header("Transforms")]
    public Transform leftFoot;
    public Transform rightFoot;
    public Transform feetRoot;
    public Transform body;

    [Header("Masks")]
    public LayerMask enviromentMask;

    [Header("Animation curves")]
    public AnimationCurve verticalCurive;
    public AnimationCurve horizontalCurive;
    public AnimationCurve hipCurve;

    [Header("Walk Settings")]
    public float footDistence;
    public float stepHeight;
    public float stepDistence;
    public float defultHeight;
    public float hipMovment;
    public float idleTime;
    public float footHeight;
    public float footLength;
    public float hitCylinderRadius;

    [Header("State")]
    public bool jumping = false;


    private Vector3 lastPosition;
    private float totalDistence = 0;
    private float idleTimer = 0;
    private float leftReturnTimer = -1;
    private float rightReturnTimer = -1;
    private float bodyReturnTimer = -1;

    private Vector2 leftStart;
    private Vector2 rightStart;
    private Vector2 leftTimer;
    private Vector2 rightTimer;
    private float bodyStart;
    private float bodyEnd;
    private float bodyTimer;



    void Update()
    {
        float distance = Vector3.Distance(transform.position, lastPosition) / stepDistence;
        totalDistence += distance / 2;
        lastPosition = transform.position;
        if (0 < distance)
            idleTimer = idleTime;
        else
            idleTimer -= Time.deltaTime;
        Vector3 leftFootPosition;
        Vector3 rightFootPosition;
        Vector3 leftFootAngle = Vector3.zero;
        Vector3 rightFootAngle = Vector3.zero;
        float bodyPosition = body.localPosition.y;
        #region walk cycle
        if (0 < idleTimer && !jumping) {
            leftFootPosition = new Vector3(footDistence, stepHeight * verticalCurive.Evaluate(totalDistence), stepDistence * horizontalCurive.Evaluate(totalDistence));
            rightFootPosition = new Vector3(-footDistence, stepHeight * verticalCurive.Evaluate(totalDistence - .5f), stepDistence * horizontalCurive.Evaluate(totalDistence - .5f));
            bodyPosition = hipCurve.Evaluate(totalDistence) * hipMovment;
            leftReturnTimer = -1;
        }
        #endregion
        #region idle
        else
        {
            if (leftReturnTimer == -1)
            {
                leftReturnTimer = totalDistence % 1;
                rightReturnTimer = (totalDistence - .5f) % 1;
                bodyReturnTimer = totalDistence % .5f;
            }
            leftReturnTimer = Mathf.Clamp01(leftReturnTimer + Time.deltaTime * (leftReturnTimer < .5 ? -1 : 1));
            rightReturnTimer = Mathf.Clamp01(rightReturnTimer + Time.deltaTime * (rightReturnTimer < .5 ? -1 : 1));
            bodyReturnTimer = Mathf.Clamp(bodyReturnTimer + Time.deltaTime * (bodyReturnTimer < .25 ? -1 : 1), 0, .5f);
            leftFootPosition = new Vector3(footDistence, stepHeight * verticalCurive.Evaluate(leftReturnTimer), stepDistence * horizontalCurive.Evaluate(leftReturnTimer));
            rightFootPosition = new Vector3(-footDistence, stepHeight * verticalCurive.Evaluate(rightReturnTimer), stepDistence * horizontalCurive.Evaluate(rightReturnTimer));
            bodyPosition = hipCurve.Evaluate(bodyReturnTimer) * hipMovment;
            totalDistence = 0;
        }
        #endregion
        #region clamp feet to environment
        leftFootPosition = clampToEnviromentmet(leftFootPosition, ref leftStart, ref leftTimer, distance, ref leftFootAngle);
        rightFootPosition = clampToEnviromentmet(rightFootPosition, ref rightStart, ref rightTimer, distance, ref rightFootAngle);
        #endregion
        #region move body to avoid cyledner colitions
        Vector3 bodyCastStart = transform.position + transform.forward * hitCylinderRadius;
        float floorDistence;
        Debug.DrawRay(bodyCastStart, Vector3.down * defultHeight, Color.red);
        if (Physics.Raycast(bodyCastStart, Vector3.down, out RaycastHit hit, defultHeight, enviromentMask))
        {
            floorDistence = hit.distance;
            bodyTimer += distance;
        }
        else
        {
            floorDistence = defultHeight;
            bodyTimer = 0;
        }
        bodyPosition += defultHeight - Mathf.Lerp(defultHeight, floorDistence, bodyTimer);
        #endregion
        leftFoot.localPosition = leftFootPosition;
        rightFoot.localPosition = rightFootPosition;
        leftFoot.forward = leftFootAngle;
        rightFoot.forward = rightFootAngle;
        body.localPosition = new Vector3(0, bodyPosition, 0);
    }
    private Vector3 clampToFloor(Vector3 foot, ref Vector2 start, ref Vector2 timer, float distence, ref Vector3 angle, bool setTimer = false)
    {
        Vector3 FootMax = foot;
        FootMax.y = stepHeight;
        FootMax.z += footLength;
        FootMax = feetRoot.TransformPoint(FootMax);
        Debug.DrawRay(FootMax, Vector3.down * (stepHeight + footHeight), Color.red);
        if (Physics.Raycast(FootMax, Vector3.down, out RaycastHit hit, stepHeight + footHeight, enviromentMask))
        {
            float leftFootMin = stepHeight + footHeight - hit.distance;
            start.y = Mathf.Clamp(foot.y, leftFootMin, stepHeight);
            timer.y = 0;
            angle = Vector3.ProjectOnPlane(feetRoot.forward, hit.normal).normalized;
        }
        else if (setTimer)
        {
            timer.y += distence;
            angle = feetRoot.forward;
        }
        foot.y = Mathf.Lerp(start.y, foot.y, timer.y);
        return foot;
    }
    private Vector3 clampToWall(Vector3 foot, ref Vector2 start, ref Vector2 timer, float distence, bool setTimer = false)
    {
        Vector3 FootMax = foot;
        FootMax.z = 0;
        FootMax.y -= footHeight - .1f;
        FootMax = feetRoot.TransformPoint(FootMax);
        Debug.DrawRay(FootMax, feetRoot.forward * (footLength + stepDistence), Color.red);
        if (Physics.Raycast(FootMax, feetRoot.forward, out RaycastHit hit, footLength + stepDistence, enviromentMask))
        {
            float maxDistence = hit.distance - footLength;
            start.x = Mathf.Clamp(foot.z, 0, maxDistence);
            timer.x = 0;
        }
        else if (setTimer)
        {
            timer.x += distence;
        }
        foot.z = Mathf.Lerp(start.x, foot.z, timer.x);
        return foot;
    }

    private Vector3 clampToEnviromentmet(Vector3 foot, ref Vector2 start, ref Vector2 timer, float distance, ref Vector3 angle)
    {
        Vector3 horizontalClamped = clampToWall(foot, ref start, ref timer, distance, true);
        Vector3 virticalClamped = clampToFloor(foot, ref start, ref timer, distance, ref angle, true);
        if (Vector3.Distance(foot, horizontalClamped) < Vector3.Distance(foot, virticalClamped))
            return clampToFloor(horizontalClamped, ref start, ref timer, distance, ref angle);
        return clampToWall(virticalClamped, ref start, ref timer, distance);
    }
}
