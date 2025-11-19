using UnityEngine;

public class WeponSwing : MonoBehaviour
{
    public Transform arm;
    public Transform sholder;
    public float loadSeconds;
    public float swingSeconds;
    public bool canSwing;

    Quaternion start = Quaternion.identity;
    Quaternion armRotation = Quaternion.Euler(0, 90, 0);
    Quaternion sholderRotation = Quaternion.Euler(0, 0, -90);
    float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        if (canSwing)
            timer += Time.deltaTime / loadSeconds;
        else
            timer -= Time.deltaTime / loadSeconds;
        timer = Mathf.Clamp01(timer);
        arm.localRotation = Quaternion.Slerp(start, armRotation, timer);
        sholder.localRotation = Quaternion.Slerp(start, sholderRotation, timer);

    }
}
