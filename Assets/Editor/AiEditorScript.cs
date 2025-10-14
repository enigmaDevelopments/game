using System.CodeDom;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AI))]


public class AiEditorScript : Editor
{
   private void OnSceneGUI()
   {
       AI ai = (AI)target;
       Handles.color = Color.blue;
       Handles.DrawWireDisc(ai.transform.position, Vector3.up, ai.detectionRadius);

    }
}
