using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Upgrades;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpgradeController))]
[CanEditMultipleObjects]
public class UpgradeControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Use reflection-based logic as the primary path to avoid SerializeReference quirks
        DrawControllerInspector();
    }

    private void DrawControllerInspector()
    {
        var controller = (UpgradeController)target;

        // Draw the default inspector for other fields (if any)
        // but the availableUpgrades list is managed below
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Available Upgrades (Managed)", EditorStyles.boldLabel);

        // Reflect the private field
        var listField = typeof(UpgradeController).GetField(
            "availableUpgrades",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        if (listField == null)
        {
            EditorGUILayout.HelpBox("UpgradeController.availableUpgrades field not found.", MessageType.Error);
            return;
        }

        var list = listField.GetValue(controller) as List<UpgradeBase>;
        if (list == null)
        {
            list = new List<UpgradeBase>();
            listField.SetValue(controller, list);
            EditorUtility.SetDirty(controller);
        }

        // Show current upgrades with editable name and remove (X) button
        if (list.Count == 0)
        {
            EditorGUILayout.HelpBox("No upgrades added yet.", MessageType.Info);
        }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                var upgrade = list[i];
                if (upgrade == null)
                {
                    // Cleanup nulls if any
                    list.RemoveAt(i);
                    i--;
                    EditorUtility.SetDirty(controller);
                    continue;
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    // Icon preview (optional)
                    var icon = upgrade.Icon;
                    if (icon != null)
                    {
                        GUILayout.Label(icon.texture, GUILayout.Width(32), GUILayout.Height(32));
                    }

                    // Editable name (Title). Label shows the type name for clarity
                    EditorGUI.BeginChangeCheck();
                    var newTitle = EditorGUILayout.TextField(new GUIContent(upgrade.GetType().Name, upgrade.Description), upgrade.Title);
                    if (EditorGUI.EndChangeCheck())
                    {
                        upgrade.Title = newTitle;
                        EditorUtility.SetDirty(controller);
                    }

                    // Remove button (X)
                    if (GUILayout.Button("X", GUILayout.Width(24)))
                    {
                        list.RemoveAt(i);
                        i--;
                        EditorUtility.SetDirty(controller);
                        continue;
                    }
                }
            }
        }

        EditorGUILayout.Space();
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Add Upgrade"))
            {
                ShowAddMenu(list, controller);
            }

            using (new EditorGUI.DisabledScope(list.Count == 0))
            {
                if (GUILayout.Button("Clear All"))
                {
                    list.Clear();
                    EditorUtility.SetDirty(controller);
                }
            }
        }
    }

    private void ShowAddMenu(List<UpgradeBase> current, UpgradeController controller)
    {
        var types = FindConcreteUpgradeTypes();

        // Remove types already present (no duplicates)
        var presentTypes = new HashSet<Type>(current.Where(u => u != null).Select(u => u.GetType()));
        var candidates = types.Where(t => !presentTypes.Contains(t)).ToList();

        var menu = new GenericMenu();
        if (candidates.Count == 0)
        {
            menu.AddDisabledItem(new GUIContent("No addable upgrades (all present)"));
        }
        else
        {
            foreach (var type in candidates)
            {
                var label = new GUIContent(type.FullName);
                menu.AddItem(label, false, () =>
                {
                    var instance = (UpgradeBase)Activator.CreateInstance(type);
                    // Give a default title if empty
                    if (string.IsNullOrEmpty(instance.Title))
                        instance.Title = type.Name;

                    current.Add(instance);
                    EditorUtility.SetDirty(controller);
                });
            }
        }
        menu.ShowAsContext();
    }

    private static List<Type> FindConcreteUpgradeTypes()
    {
        return TypeCache.GetTypesDerivedFrom<UpgradeBase>()
            .Where(t => !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) != null)
            .OrderBy(t => t.FullName)
            .ToList();
    }
}
