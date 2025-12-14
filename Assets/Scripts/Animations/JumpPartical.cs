using UnityEngine;

public class JumpPartical : MonoBehaviour
{
    public ParticleSystem partical;
    float last;
    // Update is called once per frame
    void Update()
    {
        float y = transform.root.position.y;
        Debug.Log(y - last);
        var main = partical.main;
        main.emitterVelocity = new Vector3(0, Mathf.Max(0, y - last), 0);
        last = y;
    }
}
