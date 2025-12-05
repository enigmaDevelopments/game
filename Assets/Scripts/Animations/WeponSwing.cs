using Unity.Mathematics;
using UnityEngine;

public class WeponSwing : MonoBehaviour
{
    [Header("Arm Parts")]
    public Transform arm;
    public Transform sholder;
    public Transform hand;
    [Header("Angles")]
    public quaternion maxSwingAngle;
    public quaternion startHandAngle;
    public quaternion endHandAngle;
    [Header("Timings")]
    public float loadSeconds;
    public float swingSeconds;
    [Header("States")]
    public bool canSwing;
    public bool swing;

    Quaternion start = Quaternion.identity;
    Quaternion armRotation = Quaternion.Euler(0, 90, 0);
    Quaternion sholderRotation = Quaternion.Euler(0, 0, -90);
    float loadTimer = 0;
    float swingTimer = 0;
    private bool reverseSwing = false;

    // Update is called once per frame
    void Update()
    {
        if (canSwing)
        {
            loadTimer += Time.deltaTime / loadSeconds;
            if (swing)
            {
                if (reverseSwing)
                    swingTimer -= Time.deltaTime / swingSeconds;
                else
                    swingTimer += Time.deltaTime / swingSeconds;
            }
            if(1 <= swingTimer)
                reverseSwing = true;

            if (swingTimer <= 0)
            {
                reverseSwing = false;
                swing = false;
            }
        }
        else
        {
            loadTimer -= Time.deltaTime / loadSeconds;
            swingTimer = 0;
        }
        loadTimer = Mathf.Clamp01(loadTimer);
        arm.localRotation = Quaternion.Slerp(start, armRotation, loadTimer);
        if (swing)
        {
            sholder.localRotation = Quaternion.Slerp(sholderRotation, maxSwingAngle, swingTimer);
            hand.localRotation = Quaternion.Slerp(startHandAngle, endHandAngle, swingTimer);
        }
        else
            sholder.localRotation = Quaternion.Slerp(start, sholderRotation, loadTimer);
        

    }
}
