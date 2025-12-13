using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class AxisBillboard : MonoBehaviour
{
    void LateUpdate()
    {
        Vector3 forward = Camera.main.transform.forward;
        Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        Vector3 localRotation = (Quaternion.Inverse(transform.parent.rotation) * rotation).eulerAngles;
        transform.localEulerAngles = new Vector3(0, localRotation.y, 0);
    }
}
 