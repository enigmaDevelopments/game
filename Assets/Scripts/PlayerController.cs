using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Debug.Log($"Horizontal: {horizontal}, Vertical: {vertical}");
        Vector3 movement = new Vector3(horizontal, 0.0f, vertical);
        movement = Vector3.ClampMagnitude(movement, 1) * speed;
        rb.linearVelocity = movement;
        float rawHorizontal = Input.GetAxisRaw("Horizontal");
        float rawVertical = Input.GetAxisRaw("Vertical");
        Vector3 raw = new Vector3(rawHorizontal, 0.0f, rawVertical);
        if (raw != Vector3.zero)
            rb.MoveRotation(Quaternion.LookRotation(movement));
    }
}
