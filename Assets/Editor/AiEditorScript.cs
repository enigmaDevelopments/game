using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AI))]

public class AiEditorScript : Editor
{
    private bool runAway = true;
    private bool detection = true;
    private bool seight = true;
    private bool hasWeapon = true;
    private float runAwayRadius = 0;
    private float detectionRadius = 0;
    private float veiwRadius = 0;
    private float attackAngle = 0;

    private void OnSceneGUI()
    {
        AI ai = (AI)target;
        if (runAway)
        {
            Handles.color = Color.red;
            Handles.DrawWireArc(ai.transform.position, Vector3.up, Vector3.forward, 360, ai.runAwayRadius);
        }
        if (detection)
        {
            Handles.color = Color.blue;
            Handles.DrawWireArc(ai.transform.position, Vector3.up, Vector3.forward, 360, ai.detectionRadius);
        }
        if (seight)
        {
            Handles.color = Color.green;
            Handles.DrawWireArc(ai.transform.position, Vector3.up, ai.transform.forward, ai.veiwAngle / 2, veiwRadius);
            Handles.DrawWireArc(ai.transform.position, Vector3.up, ai.transform.forward, -ai.veiwAngle / 2, veiwRadius);
        }
    }
    public override void OnInspectorGUI()
    {
        AI ai = (AI)target;
        serializedObject.Update();
        ai.runAway = EditorGUILayout.Toggle("Run Away", ai.runAway);
        if (ai.runAway)
        {
            if (runAway)
                runAwayRadius = Mathf.Max(0, EditorGUILayout.FloatField("Run Away Radius", ai.runAwayRadius));
            else
                runAway = true;
            ai.runAwayRadius = runAwayRadius;
        }
        else
        {
            runAway = false;
            ai.runAwayRadius = 0;
        }
        ai.detection = EditorGUILayout.Toggle("Detection", ai.detection);
        if (ai.detection)
        {
            if (detection)
                detectionRadius = Mathf.Max(0, EditorGUILayout.FloatField("Detection Radius", ai.detectionRadius));
            else
                detection = true;
            ai.detectionRadius = detectionRadius;
        }
        else
        {
            detection = false;
            ai.detectionRadius = 0;
        }
        serializedObject.ApplyModifiedProperties();

        ai.sight = EditorGUILayout.Toggle("Sight", ai.sight);
        if (ai.sight)
        {
            ai.raycast = EditorGUILayout.Toggle("Raycast", ai.raycast);
            if (ai.raycast)
            {
                SerializedProperty enviromentMask = serializedObject.FindProperty("enviromentMask");
                EditorGUILayout.PropertyField(enviromentMask);
            }
            if (seight)
                veiwRadius = Mathf.Max(0, EditorGUILayout.FloatField("Veiw Radius", ai.veiwRadius));
            else
                seight = true;
            ai.veiwRadius = veiwRadius;
            ai.veiwAngle = EditorGUILayout.Slider("Veiw Angle", ai.veiwAngle, 0, 360);
        }
        else
        {
            seight = false;
            ai.veiwRadius = 0;
            if (!ai.detection)
            {
                ai.omniscient = EditorGUILayout.Toggle("Omniscient", ai.omniscient);
                if (ai.omniscient)
                    ai.detectionRadius = float.PositiveInfinity;
            }
        }

        ai.hasWeapon = EditorGUILayout.Toggle("HasWeapon", ai.hasWeapon);
        if (ai.hasWeapon)
        {
            ai.weapon = (Weapon)EditorGUILayout.ObjectField("Weapon", ai.weapon, typeof(Weapon), true);
            if (hasWeapon)
                attackAngle = EditorGUILayout.Slider("Attack Angle", ai.attackAngle, 0, 360);
            else
                hasWeapon = true;
            ai.attackAngle = attackAngle;
        }
        else
        {
            hasWeapon = false;
            ai.attackAngle = 0;
        }

        ai.turningSpeed = EditorGUILayout.Slider("Turning Speed", ai.turningSpeed, 0, 1);
    }
}
