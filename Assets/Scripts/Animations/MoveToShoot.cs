using UnityEngine;

public class MoveToShoot : WeponAnimation
{
    public Transform arm;

    Quaternion start = Quaternion.identity;
    Quaternion shoot = Quaternion.Euler(0, 90, 0);
    protected override void moveStance()
    {
        arm.localRotation = Quaternion.Slerp(start, shoot, loadTimer);
    }
    protected override void moveAttack()
    {
        //todo
    }

}
