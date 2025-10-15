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
        DrawPropertiesExcluding(serializedObject, "runAway", "runAwayRadius", "detection", "detectionRadius");
        ai.runAway = EditorGUILayout.Toggle("Run Away", ai.runAway);
        if (ai.runAway)
            ai.runAwayRadius = EditorGUILayout.FloatField("Run Away Radius", ai.runAwayRadius);
        ai.detection = EditorGUILayout.Toggle("Detection", ai.detection);
        if (ai.detection)
            ai.detectionRadius = EditorGUILayout.FloatField("Detection Radius", ai.detectionRadius);
        serializedObject.ApplyModifiedProperties();
    }
}
