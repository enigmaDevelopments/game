using UnityEngine;


public class ShotGun : LaunchProjectile
{
    [Range(0f, 90f)]
    public float spreadX;
    [Range(0f, 90f)]
    public float spreadY;
    public int bullets;

    protected override void Shoot()
    {
        for (int i = 0; i < bullets; i++) 
        {
            Shoot(Quaternion.Euler(0, Random.Range(-spreadX,spreadX),Random.Range(-spreadY,spreadY)));
        }
    }
}