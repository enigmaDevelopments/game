using UnityEngine;


public class ShotGun : LaunchProjectile
{
    [Range(0f, 90f)]
    public float spreadX;
    [Range(0f, 90f)]
    public float spreadY;
    public int bullets;
    public bool singleHit;

    protected override void Shoot()
    {
        int id = Random.Range(int.MinValue, int.MaxValue);
        for (int i = 0; i < bullets; i++) 
        {
            Projectile projectile = Shoot(Quaternion.Euler(0, Random.Range(-spreadX,spreadX),Random.Range(-spreadY,spreadY)));
            if (singleHit)
                projectile.id = id;
        }
    }
}