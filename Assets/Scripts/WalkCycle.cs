using UnityEngine;

public class WalkCycle : MonoBehaviour
{
    public Transform leftFoot;
    public Transform rightFoot;

    public AnimationCurve verticalCurive;
    public AnimationCurve horizontalCurive;
    public float footDistence;
    public float stepHeight;
    public float stepDistence;

    private Vector3 lastPosition;
    private float totalDistence = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        totalDistence += Vector3.Distance(transform.position, lastPosition)/stepDistence;
        lastPosition = transform.position;
        Vector3 leftFootPosition = new Vector3(footDistence, stepHeight * verticalCurive.Evaluate(totalDistence), stepDistence * horizontalCurive.Evaluate(totalDistence));
        Vector3 rightFootPosition = new Vector3(-footDistence, stepHeight * verticalCurive.Evaluate(totalDistence-.5f), stepDistence * horizontalCurive.Evaluate(totalDistence - .5f));
        leftFoot.localPosition = leftFootPosition;
        rightFoot.localPosition = rightFootPosition;
    }
}
