using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AI))]

public class AiEditorScript : Editor
{
    private bool runAway = true;
    private bool detection = true;
    private bool seight = true;
    private bool lookAtPlayer = true;
    private bool pitch = true;
    private bool check = true;
    private float runAwayRadius = 0;
    private float detectionRadius = 0;
    private float veiwRadius = 0;
    private float turningSpeed = 0;
    private float maxPitch = 0;
    private float checksPerSecond = 0;

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
            bool search = EditorGUILayout.Toggle("Search On Hit", ai.search);
            if (search)
            {
                ai.health = ai.GetComponent<Enemy>();
                search = ai.health != null;
            }
            ai.search = search;
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

        bool hasWeapon = EditorGUILayout.Toggle("Has Wepon", ai.hasWeapon);
        if (hasWeapon)
        {
            ai.attackController = ai.GetComponent<AttackController>();
            hasWeapon = ai.health != null;
        }
        else
            ai.attackController = null;
        ai.hasWeapon = hasWeapon;

        ai.lookAtPlayer = EditorGUILayout.Toggle("Look At Player", ai.lookAtPlayer);
        if (ai.lookAtPlayer)
        {
            if (lookAtPlayer)
            {
                turningSpeed = Mathf.Max(0, EditorGUILayout.FloatField("Turning Speed", ai.turningSpeed));
                ai.offsetVector = EditorGUILayout.Vector3Field("Rotational Offset Vector", ai.offsetVector);
            } 
            else
                lookAtPlayer = true;
            ai.pitchRotation = EditorGUILayout.Toggle("Has Pitch Rotation", ai.pitchRotation);
            if (ai.pitchRotation)
            {
                if (pitch)
                    maxPitch = EditorGUILayout.Slider("Max Pitch", ai.pitchMaximum, 0, 180);
                pitch = true;
                ai.pitchMaximum = maxPitch;
            }
            else
            {
                ai.pitchMaximum = 0;
                pitch = false;
            }
            ai.turningSpeed = turningSpeed;
            ai.offsetAngle = Quaternion.Inverse(Quaternion.Euler(ai.offsetVector));
            SerializedProperty rotationProp = serializedObject.FindProperty("rotaionTransform");
            EditorGUILayout.PropertyField(rotationProp);
            
        }
        else
        {
            lookAtPlayer = false;
            ai.turningSpeed = 0;
            ai.rotaionTransform = ai.transform;
            ai.offsetAngle = Quaternion.identity;
        }

        SerializedProperty headProp = serializedObject.FindProperty("head");
        if (ai.head == null)
            ai.head = ai.transform;
        EditorGUILayout.PropertyField(headProp);

        if (ai.detection || ai.sight)
        {
            if (check)
                checksPerSecond = EditorGUILayout.Slider("Checks Per Second", ai.checksPerSecond, 0, 1 / Time.fixedDeltaTime);
            else
                check = true;
            ai.checksPerSecond = checksPerSecond; 
        }
        else
        {
            ai.checksPerSecond = ai.omniscient? 1/Time.fixedDeltaTime:0;
            check = false;
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(ai);
    }
}
