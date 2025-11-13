using System.Threading;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class WalkCycle : MonoBehaviour
{
    [Header("Transforms")]
    public Transform leftFoot;
    public Transform rightFoot;
    public Transform feetRoot;

    [Header("Masks")]
    public LayerMask enviromentMask;

    [Header("Animation curves")]
    public AnimationCurve verticalCurive;
    public AnimationCurve horizontalCurive;

    [Header("Walk Settings")]
    public float footDistence;
    public float stepHeight;
    public float stepDistence;
    public float idleTime;
    public float footHeight;

    private Vector3 lastPosition;
    private float totalDistence = 0;
    private float idleTimer = 0;
    private float leftReturnTimer = -1;
    private float rightReturnTimer = -1;
    

    void Update()
    {
        float distance = Vector3.Distance(transform.position, lastPosition) / stepDistence;
        totalDistence += distance;
        lastPosition = transform.position;
        if (0 < distance)
            idleTimer = idleTime;
        else
            idleTimer -= Time.deltaTime;
        Vector3 leftFootPosition;
        Vector3 rightFootPosition;
        #region walk cycle
        if (0 < idleTimer) {
            leftFootPosition = new Vector3(footDistence, stepHeight * verticalCurive.Evaluate(totalDistence), stepDistence * horizontalCurive.Evaluate(totalDistence));
            rightFootPosition = new Vector3(-footDistence, stepHeight * verticalCurive.Evaluate(totalDistence - .5f), stepDistence * horizontalCurive.Evaluate(totalDistence - .5f));
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
            }
            leftReturnTimer = Mathf.Clamp01(leftReturnTimer + Time.deltaTime * (leftReturnTimer < .5? -1:1));
            rightReturnTimer = Mathf.Clamp01(rightReturnTimer + Time.deltaTime * (rightReturnTimer < .5 ? -1 : 1));
            leftFootPosition = new Vector3(footDistence, stepHeight * verticalCurive.Evaluate(leftReturnTimer), stepDistence * horizontalCurive.Evaluate(leftReturnTimer));
            rightFootPosition = new Vector3(-footDistence, stepHeight * verticalCurive.Evaluate(rightReturnTimer), stepDistence * horizontalCurive.Evaluate(rightReturnTimer));

            totalDistence = 0;
        }
        #endregion
        #region Keep feet on ground
        Vector3 leftFootMax = leftFootPosition;
        Vector3 rightFootMax = rightFootPosition;
        leftFootMax.y = rightFootMax.y = stepHeight;
        leftFootMax = feetRoot.TransformPoint(leftFootMax);
        rightFootMax = feetRoot.TransformPoint(rightFootMax);
        RaycastHit leftHitInfo;
        RaycastHit rightHitInfo;
        Physics.Raycast(leftFootMax, Vector3.down, out leftHitInfo,float.PositiveInfinity, enviromentMask);
        Physics.Raycast(rightFootMax, Vector3.down, out rightHitInfo, float.PositiveInfinity, enviromentMask);
        float leftFootMin = stepHeight + footHeight - leftHitInfo.distance;
        float rightFootMin = stepHeight + footHeight - rightHitInfo.distance;
        Debug.DrawRay(leftFootMax, Vector3.down*100,Color.blue);
        Debug.DrawRay(rightFootMax, Vector3.down * 100, Color.blue);
        leftFootPosition.y = Mathf.Clamp(leftFootPosition.y, leftFootMin, stepHeight);
        rightFootPosition.y = Mathf.Clamp(rightFootPosition.y, rightFootMin, stepHeight);
        #endregion
        leftFoot.localPosition = leftFootPosition;
        rightFoot.localPosition = rightFootPosition;
    }
}
