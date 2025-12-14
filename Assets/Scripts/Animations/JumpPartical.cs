using UnityEngine;

public class JumpPartical : MonoBehaviour
{
    public ParticleSystem partical;
    public ParticleSystem.MainModule main;
    float lastPositon;

    private void Start()
    {
        main = partical.main;
        lastPositon = transform.root.position.y;
    }
    void Update()
    {
        float y = transform.root.position.y;
        main.emitterVelocity = new Vector3(0, Mathf.Max(0, (y - lastPositon)/Time.deltaTime), 0);
        lastPositon = y;
    }
}
