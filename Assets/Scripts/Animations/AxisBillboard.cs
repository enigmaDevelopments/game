using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class AxisBillboard : MonoBehaviour
{
    void LateUpdate()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 rotation = (Quaternion.Inverse(transform.parent.rotation) * Quaternion.LookRotation(Camera.main.transform.forward)).eulerAngles;
        float x = rotation.x;
        float y = rotation.y;
        float z = rotation.z;
        transform.localEulerAngles = new Vector3(0,y,-z);
        transform.parent.localEulerAngles = new Vector3(90-x,0,0);
    }
}
