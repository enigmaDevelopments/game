using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AI))]


public class AiEditorScript : Editor
{
    private void OnSceneGUI()
    {
        AI ai = (AI)target;
        Handles.color = Color.blue;
        Handles.DrawWireArc(ai.transform.position, Vector3.up, Vector3.forward, 360, ai.detectionRadius);
    }

    public override void OnInspectorGUI()
    {
        AI ai = (AI)target;
        serializedObject.Update();
        ai.runAway = EditorGUILayout.Toggle("Run Away", ai.runAway);
        if (ai.runAway)
            ai.runAwayRadius = EditorGUILayout.FloatField("Run Away Radius", ai.runAwayRadius);
        ai.detection = EditorGUILayout.Toggle("Detection", ai.detection);
        if (ai.detection)
            ai.detectionRadius = EditorGUILayout.FloatField("Detection Radius", ai.detectionRadius);
        serializedObject.ApplyModifiedProperties();

        ai.seight = EditorGUILayout.Toggle("seight", ai.seight);
        if (ai.seight)
        {
            ai.raycast = EditorGUILayout.Toggle("Raycast", ai.raycast);
            if (ai.raycast)
                ai.enviromentMask = EditorGUILayout.LayerField("Enviroment Mask", ai.enviromentMask);
            ai.veiwRadius = EditorGUILayout.FloatField("Veiw Radius", ai.veiwRadius);
            ai.veiwAngle = EditorGUILayout.Slider("Veiw Angle", ai.veiwAngle, 0, 360);
        }
        DrawPropertiesExcluding(serializedObject, "m_Script", "runAway", "runAwayRadius", "detection", "detectionRadius", "seight", "veiwAngle", "veiwRadius", "raycast", "enviromentMask");
    }
}
