using UnityEngine;

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
    public float footLength;

    private Vector3 lastPosition;
    private float totalDistence = 0;
    private float idleTimer = 0;
    private float leftReturnTimer = -1;
    private float rightReturnTimer = -1;
    

    void Update()
    {
        float distance = Vector3.Distance(transform.position, lastPosition) / stepDistence / 2;
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
        {
            Vector3 leftFootMax = leftFootPosition;
            Vector3 rightFootMax = rightFootPosition;
            leftFootMax.y = rightFootMax.y = stepHeight;
            leftFootMax.z += footLength;
            rightFootMax.z += footLength;
            leftFootMax = feetRoot.TransformPoint(leftFootMax);
            rightFootMax = feetRoot.TransformPoint(rightFootMax);
            RaycastHit leftHitInfo;
            RaycastHit rightHitInfo;
            Physics.Raycast(leftFootMax, Vector3.down, out leftHitInfo,float.PositiveInfinity, enviromentMask);
            Physics.Raycast(rightFootMax, Vector3.down, out rightHitInfo, float.PositiveInfinity, enviromentMask);
            float leftFootMin = stepHeight + footHeight - leftHitInfo.distance;
            float rightFootMin = stepHeight + footHeight - rightHitInfo.distance;
            leftFootPosition.y = Mathf.Clamp(leftFootPosition.y, leftFootMin, stepHeight);
            rightFootPosition.y = Mathf.Clamp(rightFootPosition.y, rightFootMin, stepHeight);
        }
        #endregion
        #region Feet hit walls
        {
            Vector3 leftFootMax = leftFootPosition;
            Vector3 rightFootMax = rightFootPosition;
            leftFootMax.z = rightFootMax.z = 0;
            leftFootMax = feetRoot.TransformPoint(leftFootMax);
            rightFootMax = feetRoot.TransformPoint(rightFootMax);
            if (Physics.Raycast(leftFootMax, feetRoot.forward, out RaycastHit leftHitInfo, footLength, enviromentMask))
            {
                float maxDistence = leftHitInfo.distance - footLength;
                leftFootPosition.z = Mathf.Clamp(leftFootPosition.z, 0, maxDistence);
            }
            if (Physics.Raycast(rightFootMax, feetRoot.forward, out RaycastHit rightHitInfo, footLength, enviromentMask))
            {
                float maxDistence = rightHitInfo.distance - footLength;
                rightFootPosition.z = Mathf.Clamp(rightFootPosition.z, 0, maxDistence);
            }
        }

        #endregion

        leftFoot.localPosition = leftFootPosition;
        rightFoot.localPosition = rightFootPosition;
    }
}
