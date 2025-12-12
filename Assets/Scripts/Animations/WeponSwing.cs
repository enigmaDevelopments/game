using Unity.Mathematics;
using UnityEngine;

public class WeponSwing : WeponAnimation
{
    [Header("Arm Parts")]
    public Transform arm;
    public Transform sholder;
    public Transform hand;
    [Header("Angles")]
    public quaternion maxSwingAngle;
    public quaternion startHandAngle;
    public quaternion endHandAngle;

    Quaternion start = Quaternion.identity;
    Quaternion armRotation = Quaternion.Euler(0, 90, 0);
    Quaternion sholderRotation = Quaternion.Euler(0, 0, -90);


    protected override void moveStance()
    {
        arm.localRotation = Quaternion.Slerp(start, armRotation, loadTimer);
        sholder.localRotation = Quaternion.Slerp(start, sholderRotation, loadTimer);
    }
    protected override void moveAttack()
    {
        sholder.localRotation = Quaternion.Slerp(sholderRotation, maxSwingAngle, attackTimer);
        hand.localRotation = Quaternion.Slerp(startHandAngle, endHandAngle, attackTimer);
    }


}
