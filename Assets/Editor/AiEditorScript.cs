using System.CodeDom;
using System.Security.Cryptography.X509Certificates;
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
        EditorGUI.BeginChangeCheck();
        ai.runAway = EditorGUILayout.Toggle("Run Away", ai.runAway);
        if (ai.runAway)
            ai.runAwayRadius = EditorGUILayout.FloatField("Run Away Radius", ai.runAwayRadius);
        ai.detectionRadius = EditorGUILayout.FloatField("Detection Radius", ai.detectionRadius);
        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(ai);
    }
}
