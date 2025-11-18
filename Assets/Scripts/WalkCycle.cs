using System.Xml;
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
    public float heelLength;
    public float hitCylinderRadius;

    [Header("State")]
    public bool jumping = false;


    private Vector3 lastPosition;
    private float totalDistence = 0;
    private float totalYDistence = 0;
    float LastBodyPosition = 0;
    private float idleTimer = 0;
    private float leftReturnTimer = -1;
    private float rightReturnTimer = -1;
    private float bodyReturnTimer = -1;



    void Update()
    {
        float distance = Vector3.Distance(transform.position, lastPosition) / stepDistence;
        totalDistence += distance / 2;
        if (0 < distance)
            idleTimer = idleTime;
        else
            idleTimer -= Time.deltaTime;
        Vector3 leftFootPosition;
        Vector3 rightFootPosition;
        Vector3 leftFootAngle = feetRoot.forward;
        Vector3 rightFootAngle = feetRoot.forward;
        float bodyPosition;
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
        #region clamp feet to environmenta
        leftFootPosition = clampToEnviromentmet(leftFootPosition, ref leftFootAngle);
        rightFootPosition = clampToEnviromentmet(rightFootPosition, ref rightFootAngle);
        #endregion
        #region move body to avoid cyledner colitions
        if (jumping)
            totalYDistence = 0;
        else
        {
            totalYDistence -=  transform.InverseTransformPoint(lastPosition).y;
            bodyPosition -= totalYDistence;
        }
        Vector3 bodyCastStart = body.position + transform.forward * hitCylinderRadius;
        if (Physics.Raycast(bodyCastStart, Vector3.down, out RaycastHit hit, float.PositiveInfinity, enviromentMask))
        {
            Vector3 bodyTarget = hit.point;
            bodyTarget.y += defultHeight + totalYDistence;
            bodyTarget = body.InverseTransformPoint(bodyTarget);
            Debug.Log(bodyTarget.y);
            float horizontalDisternce = Vector2.Distance(transform.position, lastPosition);
            LastBodyPosition = Mathf.MoveTowards(LastBodyPosition, bodyTarget.y, distance * stepDistence);
            bodyPosition += LastBodyPosition;
        }
        #endregion
        leftFoot.localPosition = leftFootPosition;
        rightFoot.localPosition = rightFootPosition;
        leftFoot.forward = leftFootAngle;
        rightFoot.forward = rightFootAngle;
        body.localPosition = new Vector3(0, bodyPosition, 0);

        lastPosition = transform.position;
    }
    private Vector3 clampToFloor(Vector3 foot, ref Vector3 angle)
    {
        float[] offsets = new float[] {0, footLength, heelLength};
        foreach (float offset in offsets)
        {
            Vector3 FootMax = foot;
            FootMax.y = stepHeight;
            FootMax.z += offset;
            FootMax = feetRoot.TransformPoint(FootMax);
            Debug.DrawRay(FootMax, Vector3.down * (stepHeight + footHeight), Color.red);
            if (Physics.Raycast(FootMax, Vector3.down, out RaycastHit hit, stepHeight + footHeight, enviromentMask))
            {
                float footMin = stepHeight + footHeight - hit.distance;
                foot.y = Mathf.Clamp(foot.y, footMin, stepHeight);
                angle = Vector3.ProjectOnPlane(feetRoot.forward, hit.normal).normalized;
            }
        }
        return foot;
    }
    private Vector3 clampToWall(Vector3 foot)
    {
        Vector3 FootMax = foot;
        FootMax.z = 0;
        FootMax.y -= footHeight - .1f;
        FootMax = feetRoot.TransformPoint(FootMax);
        Debug.DrawRay(FootMax, feetRoot.forward * (footLength + stepDistence), Color.red);
        if (Physics.Raycast(FootMax, feetRoot.forward, out RaycastHit hit, footLength + stepDistence, enviromentMask))
        {
            float maxDistence = hit.distance - footLength;
            foot.z = Mathf.Clamp(foot.z, 0, maxDistence);
        }
        return foot;
    }

    private Vector3 clampToEnviromentmet(Vector3 foot, ref Vector3 angle)
    {
        Vector3 virticalClamped = clampToFloor(foot, ref angle);

        return clampToWall(virticalClamped);
    }
}
