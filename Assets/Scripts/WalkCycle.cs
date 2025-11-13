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
        #region clamp feet to environment
        leftFootPosition = clampToWall(leftFootPosition);
        rightFootPosition = clampToWall(rightFootPosition);

        leftFootPosition = clampToFloor(leftFootPosition);
        rightFootPosition = clampToFloor(rightFootPosition);
        #endregion

        leftFoot.localPosition = leftFootPosition;
        rightFoot.localPosition = rightFootPosition;
    }
    private Vector3 clampToFloor(Vector3 foot)
    {
        Vector3 FootMax = foot;
        FootMax.y = stepHeight;
        FootMax.z += footLength - .001f;
        FootMax = feetRoot.TransformPoint(FootMax);
        if (Physics.Raycast(FootMax, Vector3.down, out RaycastHit hit, float.PositiveInfinity, enviromentMask))
        {
            float leftFootMin = stepHeight + footHeight - hit.distance;
            foot.y = Mathf.Clamp(foot.y, leftFootMin, stepHeight);
        }
        return foot;
    }
    private Vector3 clampToWall(Vector3 foot)
    {
        Vector3 FootMax = foot;
        FootMax.z = 0;
        FootMax = feetRoot.TransformPoint(FootMax);
        if (Physics.Raycast(FootMax, feetRoot.forward, out RaycastHit hit, footLength, enviromentMask))
        {
            float maxDistence = hit.distance - footLength;
            foot.z = Mathf.Clamp(foot.z, 0, maxDistence);
        }
        return foot;
    }


}
