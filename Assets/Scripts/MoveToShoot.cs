using System.Threading;
using UnityEngine;

public class MoveToShoot : MonoBehaviour
{
    public Transform arm;
    public float seconds;
    public bool canShoot;

    Quaternion start = Quaternion.identity;
    Quaternion shoot = Quaternion.Euler(0, 90, 0);
    float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        if (canShoot)
            timer += Time.deltaTime / seconds;
        else
            timer -= Time.deltaTime / seconds;
        timer = Mathf.Clamp01(timer);
        arm.localRotation = Quaternion.Slerp(start, shoot, timer);
    }
}
