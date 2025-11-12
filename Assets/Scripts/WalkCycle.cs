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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 leftFootPosition = new Vector3(footDistence, stepHeight * verticalCurive.Evaluate(Time.time), stepDistence * horizontalCurive.Evaluate(Time.time));
        Vector3 rightFootPosition = new Vector3(-footDistence, stepHeight * verticalCurive.Evaluate(Time.time-.5f), stepDistence * horizontalCurive.Evaluate(Time.time-.5f));
        leftFoot.localPosition = leftFootPosition;
        rightFoot.localPosition = rightFootPosition;
    }
}
