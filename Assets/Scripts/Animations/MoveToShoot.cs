using UnityEngine;

public class MoveToShoot : WeponAnimation
{
    public Transform arm;

    Quaternion start = Quaternion.identity;
    Quaternion shoot = Quaternion.Euler(0, 90, 0);
    protected override void moveStance()
    {
        Debug.Log("stance");
        Debug.Log(shoot);
        arm.localRotation = Quaternion.Slerp(start, shoot, loadTimer);
    }
    protected override void moveAttack()
    {
        //todo
    }

}
