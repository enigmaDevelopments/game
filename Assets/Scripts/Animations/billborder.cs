using UnityEngine;

public class billborder : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.forward = (Camera.main.transform.position - transform.position).normalized;
    }
}
