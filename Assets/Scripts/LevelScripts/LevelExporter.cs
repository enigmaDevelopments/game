using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class LevelExporter : EditorWindow
{
    static int width = 80;
    static int height = 80;
    static float cell = 1f;

    // Mapping of object name patterns to their symbols
    private static Dictionary<string, char> objectSymbolMap = new Dictionary<string, char>()
    {
        { "Wall_Long", 'W' },
        { "Wall", '#' },
        { "Divider_Long", 'D' },
        { "Divider", 'd' },
        { "Pillar", 'P' },
        { "Ramp", 'R' },
        { "Crate", 'C' },
        { "Barrel", 'B' },
        { "LightStrip", 'L' },
        { "Door", '@' }
    };

    [MenuItem("Tools/Export Level Layout")]
    public static void ShowWindow()
    {
        LevelExporter window = (LevelExporter)EditorWindow.GetWindow(typeof(LevelExporter));
        window.titleContent = new GUIContent("Level Exporter");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Export Settings", EditorStyles.boldLabel);
        
        EditorGUILayout.Space();
        
        EditorGUILayout.HelpBox(
            "This tool will scan the current scene and export all objects based on their names.\n\n" +
            "Recognized objects:\n" +
            "• Wall (#), Wall_Long (W)\n" +
            "• Divider (d), Divider_Long (D)\n" +
            "• Pillar (P), Ramp (R)\n" +
            "• Crate (C), Barrel (B)\n" +
            "• LightStrip (L), Door (@)\n" +
            "• EnemySpawner component (S)",
            MessageType.Info
        );

        EditorGUILayout.Space();

        GUILayout.Label($"Grid Size: {width} x {height}", EditorStyles.label);
        GUILayout.Label($"Cell Size: {cell}", EditorStyles.label);

        EditorGUILayout.Space();

        if (GUILayout.Button("EXPORT LEVEL", GUILayout.Height(40)))
        {
            Export();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Preview Scene Objects"))
        {
            PreviewSceneObjects();
        }
    }

    static void Mark(GameObject obj, char symbol, char[,] grid)
    {
        Vector3 pos = obj.transform.position;
        int x = Mathf.RoundToInt(pos.x / cell);
        int y = Mathf.RoundToInt(pos.z / cell);

        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            grid[x, y] = symbol;
        }
        else
        {
            Debug.LogWarning($"Object '{obj.name}' at position {pos} is outside grid bounds ({x}, {y})");
        }
    }

    static char GetSymbolForObject(GameObject obj)
    {
        // Check object name against our mapping (check longer names first to avoid partial matches)
        string objName = obj.name.Replace("(Clone)", "").Trim();
        
        // Check exact matches and partial matches in priority order
        foreach (var kvp in objectSymbolMap)
        {
            if (objName.Contains(kvp.Key))
            {
                return kvp.Value;
            }
        }
        
        return '\0'; // null character means not recognized
    }

    void Export()
    {
        char[,] grid = new char[width, height];
        int objectCount = 0;
        Dictionary<char, int> symbolCounts = new Dictionary<char, int>();

        // Fill with dots
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                grid[x, y] = '.';

        // Find all GameObjects in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        
        Debug.Log($"Scanning {allObjects.Length} objects in scene...");

        foreach (var obj in allObjects)
        {
            // Check if it's an EnemySpawner component
            if (obj.GetComponent<EnemySpawner>() != null)
            {
                Mark(obj, 'S', grid);
                objectCount++;
                if (!symbolCounts.ContainsKey('S'))
                    symbolCounts['S'] = 0;
                symbolCounts['S']++;
                continue;
            }

            // Check if the object name matches any of our prefab types
            char symbol = GetSymbolForObject(obj);
            if (symbol != '\0')
            {
                Mark(obj, symbol, grid);
                objectCount++;
                if (!symbolCounts.ContainsKey(symbol))
                    symbolCounts[symbol] = 0;
                symbolCounts[symbol]++;
            }
        }

        // Assemble ASCII
        StringBuilder sb = new StringBuilder();
        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
                sb.Append(grid[x, y]);
            sb.Append("\n");
        }

        string output = sb.ToString();
        string exportPath = Application.dataPath + "/LevelExport.txt";
        File.WriteAllText(exportPath, output);
        
        // Create summary report
        StringBuilder report = new StringBuilder();
        report.AppendLine("=== LEVEL EXPORT COMPLETE ===");
        report.AppendLine($"Exported to: {exportPath}");
        report.AppendLine($"Total objects exported: {objectCount}");
        report.AppendLine("\nObject breakdown:");
        
        foreach (var kvp in symbolCounts)
        {
            string objectType = GetObjectTypeFromSymbol(kvp.Key);
            report.AppendLine($"  {objectType} ({kvp.Key}): {kvp.Value}");
        }

        Debug.Log(report.ToString());
        Debug.Log("Level layout preview:\n" + output.Substring(0, Mathf.Min(500, output.Length)) + "...");
        
        EditorUtility.DisplayDialog("Export Complete", 
            $"Level exported successfully!\n\n{objectCount} objects exported to:\n{exportPath}\n\nCheck the Console for details.", 
            "OK");
    }

    void PreviewSceneObjects()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        Dictionary<string, int> objectCounts = new Dictionary<string, int>();
        List<string> recognizedObjects = new List<string>();
        List<string> unrecognizedObjects = new List<string>();

        foreach (var obj in allObjects)
        {
            string objName = obj.name.Replace("(Clone)", "").Trim();
            
            // Check if it's an EnemySpawner
            if (obj.GetComponent<EnemySpawner>() != null)
            {
                recognizedObjects.Add($"{obj.name} (EnemySpawner) at {obj.transform.position}");
                continue;
            }

            // Check if recognized
            char symbol = GetSymbolForObject(obj);
            if (symbol != '\0')
            {
                string key = $"{GetObjectTypeFromSymbol(symbol)} ({symbol})";
                if (!objectCounts.ContainsKey(key))
                    objectCounts[key] = 0;
                objectCounts[key]++;
            }
            else
            {
                // Only show root level objects or important ones
                if (obj.transform.parent == null && !objName.Contains("Camera") && !objName.Contains("Light"))
                {
                    unrecognizedObjects.Add(objName);
                }
            }
        }

        StringBuilder preview = new StringBuilder();
        preview.AppendLine("=== SCENE OBJECTS PREVIEW ===\n");
        preview.AppendLine("RECOGNIZED OBJECTS:");
        
        foreach (var kvp in objectCounts)
        {
            preview.AppendLine($"  {kvp.Key}: {kvp.Value}");
        }

        if (recognizedObjects.Count > 0)
        {
            preview.AppendLine($"\nENEMY SPAWNERS: {recognizedObjects.Count}");
            foreach (var obj in recognizedObjects)
            {
                preview.AppendLine($"  {obj}");
            }
        }

        if (unrecognizedObjects.Count > 0)
        {
            preview.AppendLine($"\nUNRECOGNIZED OBJECTS (will be ignored): {unrecognizedObjects.Count}");
            foreach (var obj in unrecognizedObjects.Take(20))
            {
                preview.AppendLine($"  {obj}");
            }
            if (unrecognizedObjects.Count > 20)
                preview.AppendLine($"  ... and {unrecognizedObjects.Count - 20} more");
        }

        Debug.Log(preview.ToString());
        EditorUtility.DisplayDialog("Scene Preview", 
            "Scene objects scanned. Check the Console for a full list.", 
            "OK");
    }

    static string GetObjectTypeFromSymbol(char symbol)
    {
        switch (symbol)
        {
            case '#': return "Wall";
            case 'W': return "Wall_Long";
            case 'd': return "Divider";
            case 'D': return "Divider_Long";
            case 'P': return "Pillar";
            case 'R': return "Ramp";
            case 'C': return "Crate";
            case 'B': return "Barrel";
            case 'L': return "LightStrip";
            case '@': return "Door";
            case 'S': return "EnemySpawner";
            default: return "Unknown";
        }
    }
}
