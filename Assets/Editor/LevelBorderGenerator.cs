using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class LevelBorderGenerator : EditorWindow
{
    // Scene reference
    private SceneAsset targetScene;

    // Prefab references
    private GameObject wallPrefab;
    private GameObject wallLongPrefab;

    // Border settings
    private float borderAreaSize = 80f; // Size of the bordered area
    private float sizeMultiplier = 1f; // Multiplier to adjust from terrain size
    private float wallHeight = 0f; // Y position for walls

    // Auto-detected terrain info
    private bool terrainDetected = false;
    private float detectedTerrainSize = 0f;
    private Vector3 terrainCenter = Vector3.zero;

    // Prefab dimensions
    private Vector3 wallPrefabSize = Vector3.one;
    private Vector3 wallLongPrefabSize = Vector3.one;

    // Border configuration
    private bool useTopBorder = true;
    private bool useBottomBorder = true;
    private bool useLeftBorder = true;
    private bool useRightBorder = true;

    // Prefab selection
    private bool useLongWallsForHorizontal = true;
    private bool useLongWallsForVertical = false;

    // Organization
    private string borderParentName = "LevelBorder";
    private bool createParentObject = true;

    [MenuItem("Tools/Generate Level Border")]
    public static void ShowWindow()
    {
        LevelBorderGenerator window = (LevelBorderGenerator)EditorWindow.GetWindow(typeof(LevelBorderGenerator));
        window.titleContent = new GUIContent("Border Generator");
        window.minSize = new Vector2(400, 600);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Border Generator", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // Scene selection
        EditorGUILayout.LabelField("Scene Setup", EditorStyles.boldLabel);
        SceneAsset previousScene = targetScene;
        targetScene = EditorGUILayout.ObjectField("Target Scene", targetScene, typeof(SceneAsset), false) as SceneAsset;

        // Auto-detect terrain when scene changes
        if (targetScene != previousScene && targetScene != null)
        {
            DetectTerrainSize();
        }

        // Show detected terrain info
        if (terrainDetected)
        {
            EditorGUILayout.HelpBox($"Detected Terrain Size: {detectedTerrainSize} units\nTerrain Center: ({terrainCenter.x:F2}, {terrainCenter.y:F2}, {terrainCenter.z:F2})", MessageType.Info);
        }

        EditorGUILayout.Space();

        // Prefab selection
        EditorGUILayout.LabelField("Prefabs", EditorStyles.boldLabel);
        GameObject previousWallPrefab = wallPrefab;
        GameObject previousWallLongPrefab = wallLongPrefab;

        wallPrefab = EditorGUILayout.ObjectField("Wall Prefab", wallPrefab, typeof(GameObject), false) as GameObject;
        wallLongPrefab = EditorGUILayout.ObjectField("Wall_Long Prefab", wallLongPrefab, typeof(GameObject), false) as GameObject;

        // Auto-detect prefab sizes when prefabs change
        if (wallPrefab != previousWallPrefab && wallPrefab != null)
        {
            wallPrefabSize = GetPrefabSize(wallPrefab);
        }
        if (wallLongPrefab != previousWallLongPrefab && wallLongPrefab != null)
        {
            wallLongPrefabSize = GetPrefabSize(wallLongPrefab);
        }

        // Show prefab dimensions
        if (wallPrefab != null)
        {
            EditorGUILayout.LabelField($"Wall Size: {wallPrefabSize.x:F2} x {wallPrefabSize.y:F2} x {wallPrefabSize.z:F2}");
        }
        if (wallLongPrefab != null)
        {
            EditorGUILayout.LabelField($"Wall_Long Size: {wallLongPrefabSize.x:F2} x {wallLongPrefabSize.y:F2} x {wallLongPrefabSize.z:F2}");
        }

        EditorGUILayout.Space();

        // Grid settings
        EditorGUILayout.LabelField("Border Area Settings", EditorStyles.boldLabel);

        if (terrainDetected)
        {
            sizeMultiplier = EditorGUILayout.Slider("Size Multiplier", sizeMultiplier, 0.1f, 2f);
            borderAreaSize = detectedTerrainSize * sizeMultiplier;
            EditorGUILayout.LabelField($"Border Area Size: {borderAreaSize:F2} units");
        }
        else
        {
            borderAreaSize = EditorGUILayout.FloatField("Border Area Size", borderAreaSize);
        }

        wallHeight = EditorGUILayout.FloatField("Wall Y Position", wallHeight);

        EditorGUILayout.Space();

        // Border configuration
        EditorGUILayout.LabelField("Border Configuration", EditorStyles.boldLabel);
        useTopBorder = EditorGUILayout.Toggle("Top Border", useTopBorder);
        useBottomBorder = EditorGUILayout.Toggle("Bottom Border", useBottomBorder);
        useLeftBorder = EditorGUILayout.Toggle("Left Border", useLeftBorder);
        useRightBorder = EditorGUILayout.Toggle("Right Border", useRightBorder);

        EditorGUILayout.Space();

        // Prefab usage
        EditorGUILayout.LabelField("Wall Type Selection", EditorStyles.boldLabel);
        useLongWallsForHorizontal = EditorGUILayout.Toggle("Use Long Walls for Horizontal", useLongWallsForHorizontal);
        useLongWallsForVertical = EditorGUILayout.Toggle("Use Long Walls for Vertical", useLongWallsForVertical);

        EditorGUILayout.Space();

        // Organization
        EditorGUILayout.LabelField("Organization", EditorStyles.boldLabel);
        createParentObject = EditorGUILayout.Toggle("Create Parent Object", createParentObject);
        if (createParentObject)
        {
            borderParentName = EditorGUILayout.TextField("Parent Object Name", borderParentName);
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // Validation and Execute button
        bool canGenerate = ValidateSettings();

        if (!canGenerate)
        {
            EditorGUILayout.HelpBox("Please assign a scene and at least one wall prefab to continue.", MessageType.Warning);
        }

        EditorGUI.BeginDisabledGroup(!canGenerate);

        if (GUILayout.Button("GENERATE BORDER", GUILayout.Height(40)))
        {
            GenerateBorder();
        }

        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();

        // Info box
        EditorGUILayout.HelpBox(
            "This tool will create a border of walls around your level based on terrain detection or custom area size.\n\n" +
            "Walls will be perfectly aligned without overlaps to form a perfect square.",
            MessageType.Info
        );
    }

    private void DetectTerrainSize()
    {
        if (targetScene == null)
        {
            terrainDetected = false;
            return;
        }

        // Load the scene temporarily to detect terrain
        string scenePath = AssetDatabase.GetAssetPath(targetScene);
        Scene currentScene = EditorSceneManager.GetActiveScene();
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);

        if (scene.IsValid())
        {
            // Find all terrains in the scene
            GameObject[] rootObjects = scene.GetRootGameObjects();
            Bounds totalBounds = new Bounds();
            bool foundTerrain = false;

            foreach (GameObject obj in rootObjects)
            {
                Terrain[] terrains = obj.GetComponentsInChildren<Terrain>();
                foreach (Terrain terrain in terrains)
                {
                    if (!foundTerrain)
                    {
                        totalBounds = terrain.terrainData.bounds;
                        totalBounds.center = terrain.transform.position + totalBounds.center;
                        foundTerrain = true;
                    }
                    else
                    {
                        Bounds terrainBounds = terrain.terrainData.bounds;
                        terrainBounds.center = terrain.transform.position + terrainBounds.center;
                        totalBounds.Encapsulate(terrainBounds);
                    }
                }
            }

            if (foundTerrain)
            {
                // Use the maximum dimension (X or Z) for square border
                detectedTerrainSize = Mathf.Max(totalBounds.size.x, totalBounds.size.z);
                terrainCenter = totalBounds.center;
                terrainDetected = true;
                Debug.Log($"Detected terrain size: {detectedTerrainSize} units at center: {terrainCenter}");
            }
            else
            {
                terrainDetected = false;
            }

            // Close the additively loaded scene
            EditorSceneManager.CloseScene(scene, true);
        }
        else
        {
            terrainDetected = false;
        }
    }

    private Vector3 GetPrefabSize(GameObject prefab)
    {
        if (prefab == null)
            return Vector3.one;

        // Try to get size from Renderer
        Renderer renderer = prefab.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds.size;
        }

        // Try to get size from Collider
        Collider collider = prefab.GetComponent<Collider>();
        if (collider != null)
        {
            return collider.bounds.size;
        }

        // Try children renderers
        renderer = prefab.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds.size;
        }

        // Try children colliders
        collider = prefab.GetComponentInChildren<Collider>();
        if (collider != null)
        {
            return collider.bounds.size;
        }

        Debug.LogWarning($"Could not determine size for prefab: {prefab.name}. Using default size of 1.");
        return Vector3.one;
    }

    private bool ValidateSettings()
    {
        if (targetScene == null)
            return false;

        if (wallPrefab == null && wallLongPrefab == null)
            return false;

        return true;
    }

    private void GenerateBorder()
    {
        if (!ValidateSettings())
        {
            EditorUtility.DisplayDialog("Error", "Please assign a scene and at least one wall prefab.", "OK");
            return;
        }

        // Load the target scene
        string scenePath = AssetDatabase.GetAssetPath(targetScene);
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        if (!scene.IsValid())
        {
            EditorUtility.DisplayDialog("Error", "Failed to load the target scene.", "OK");
            return;
        }

        // Re-detect terrain size in the loaded scene to ensure accuracy
        DetectTerrainInCurrentScene();

        // Create parent object if needed
        GameObject parentObject = null;
        if (createParentObject)
        {
            parentObject = new GameObject(borderParentName);
            parentObject.transform.position = terrainCenter;
            Undo.RegisterCreatedObjectUndo(parentObject, "Create Border Parent");
        }

        int wallsCreated = 0;

        // Calculate the border boundaries (centered around terrain center)
        float halfSize = borderAreaSize / 2f;

        // Generate borders - ensuring perfect alignment
        if (useBottomBorder)
            wallsCreated += GenerateHorizontalBorder(-halfSize, parentObject, "Bottom", true);

        if (useTopBorder)
            wallsCreated += GenerateHorizontalBorder(halfSize, parentObject, "Top", true);

        if (useLeftBorder)
            wallsCreated += GenerateVerticalBorder(-halfSize, parentObject, "Left", false);

        if (useRightBorder)
            wallsCreated += GenerateVerticalBorder(halfSize, parentObject, "Right", false);

        // Mark scene as dirty
        EditorSceneManager.MarkSceneDirty(scene);

        // Show completion dialog
        EditorUtility.DisplayDialog("Border Generated",
            $"Successfully generated {wallsCreated} wall objects in scene:\n{scene.name}\n\nDon't forget to save the scene!",
            "OK");

        Debug.Log($"Border generation complete: {wallsCreated} walls created in {scene.name}");
    }

    private void DetectTerrainInCurrentScene()
    {
        // Find all terrains in the current scene
        Terrain[] allTerrains = FindObjectsByType<Terrain>(FindObjectsSortMode.None);

        if (allTerrains.Length > 0)
        {
            Bounds totalBounds = new Bounds();
            bool foundFirst = false;

            foreach (Terrain terrain in allTerrains)
            {
                if (!foundFirst)
                {
                    totalBounds = terrain.terrainData.bounds;
                    totalBounds.center = terrain.transform.position + totalBounds.center;
                    foundFirst = true;
                }
                else
                {
                    Bounds terrainBounds = terrain.terrainData.bounds;
                    terrainBounds.center = terrain.transform.position + terrainBounds.center;
                    totalBounds.Encapsulate(terrainBounds);
                }
            }

            if (foundFirst)
            {
                detectedTerrainSize = Mathf.Max(totalBounds.size.x, totalBounds.size.z);
                terrainCenter = totalBounds.center;
                borderAreaSize = detectedTerrainSize * sizeMultiplier;
                terrainDetected = true;
            }
        }
    }

    private int GenerateHorizontalBorder(float zPosition, GameObject parent, string borderName, bool isHorizontal)
    {
        int count = 0;
        GameObject prefabToUse = useLongWallsForHorizontal && wallLongPrefab != null ? wallLongPrefab : wallPrefab;

        if (prefabToUse == null)
        {
            Debug.LogWarning($"No prefab available for {borderName} border");
            return 0;
        }

        Vector3 prefabSize = useLongWallsForHorizontal && wallLongPrefab != null ? wallLongPrefabSize : wallPrefabSize;

        // Determine the wall depth (length along the border direction)
        float wallDepth = isHorizontal ? prefabSize.x : prefabSize.z;

        // Calculate how many walls we need
        float halfSize = borderAreaSize / 2f;
        float totalLength = borderAreaSize;
        int wallCount = Mathf.CeilToInt(totalLength / wallDepth);

        // Adjust to make walls fit perfectly
        float adjustedWallDepth = totalLength / wallCount;

        // Start from the left edge
        float startX = -halfSize + adjustedWallDepth / 2f;

        for (int i = 0; i < wallCount; i++)
        {
            float xPosition = startX + (i * adjustedWallDepth);
            Vector3 localPosition = new Vector3(xPosition, wallHeight, zPosition);
            Vector3 worldPosition = parent != null ? parent.transform.position + localPosition : terrainCenter + localPosition;
            GameObject wall = PrefabUtility.InstantiatePrefab(prefabToUse) as GameObject;

            if (wall != null)
            {
                wall.transform.position = worldPosition;
                wall.transform.rotation = Quaternion.identity;

                // Scale to fit perfectly if needed
                if (Mathf.Abs(adjustedWallDepth - wallDepth) > 0.001f)
                {
                    Vector3 scale = wall.transform.localScale;
                    scale.x *= (adjustedWallDepth / wallDepth);
                    wall.transform.localScale = scale;
                }

                wall.name = $"{prefabToUse.name}_{borderName}_{i}";

                if (parent != null)
                {
                    wall.transform.SetParent(parent.transform);
                }

                Undo.RegisterCreatedObjectUndo(wall, $"Create {borderName} Border Wall");
                count++;
            }
        }

        return count;
    }

    private int GenerateVerticalBorder(float xPosition, GameObject parent, string borderName, bool isVertical)
    {
        int count = 0;
        GameObject prefabToUse = useLongWallsForVertical && wallLongPrefab != null ? wallLongPrefab : wallPrefab;

        if (prefabToUse == null)
        {
            Debug.LogWarning($"No prefab available for {borderName} border");
            return 0;
        }

        Vector3 prefabSize = useLongWallsForVertical && wallLongPrefab != null ? wallLongPrefabSize : wallPrefabSize;

        // Determine the wall depth (length along the border direction)
        // For vertical walls rotated 90 degrees, X becomes Z
        float wallDepth = useLongWallsForVertical && wallLongPrefab != null ? wallLongPrefabSize.x : wallPrefabSize.x;

        // Calculate how many walls we need
        float halfSize = borderAreaSize / 2f;

        // Get horizontal wall thickness to extend into corners
        GameObject horizontalPrefab = useLongWallsForHorizontal && wallLongPrefab != null ? wallLongPrefab : wallPrefab;
        Vector3 horizontalSize = useLongWallsForHorizontal && wallLongPrefab != null ? wallLongPrefabSize : wallPrefabSize;
        float horizontalThickness = horizontalSize.z;

        // Get vertical wall thickness
        Vector3 verticalSize = useLongWallsForVertical && wallLongPrefab != null ? wallLongPrefabSize : wallPrefabSize;
        float verticalThickness = verticalSize.z;

        // Calculate total length for vertical wall - extend to meet horizontal walls
        // Add half of horizontal thickness on each end to ensure corners meet
        float totalLength = borderAreaSize + (useTopBorder ? horizontalThickness : 0) + (useBottomBorder ? horizontalThickness : 0);
        int wallCount = Mathf.CeilToInt(totalLength / wallDepth);

        if (wallCount <= 0)
            wallCount = 1;

        // Adjust to make walls fit perfectly
        float adjustedWallDepth = totalLength / wallCount;

        // Start position - extend beyond the border edge to meet horizontal walls
        float startZ = -halfSize - (useBottomBorder ? horizontalThickness : 0) + adjustedWallDepth / 2f;

        for (int i = 0; i < wallCount; i++)
        {
            float zPosition = startZ + (i * adjustedWallDepth);

            Vector3 localPosition = new Vector3(xPosition, wallHeight, zPosition);
            Vector3 worldPosition = parent != null ? parent.transform.position + localPosition : terrainCenter + localPosition;
            GameObject wall = PrefabUtility.InstantiatePrefab(prefabToUse) as GameObject;

            if (wall != null)
            {
                wall.transform.position = worldPosition;

                // Rotate vertical walls 90 degrees
                wall.transform.rotation = Quaternion.Euler(0, 90, 0);

                // Scale to fit perfectly if needed
                if (Mathf.Abs(adjustedWallDepth - wallDepth) > 0.001f)
                {
                    Vector3 scale = wall.transform.localScale;
                    scale.x *= (adjustedWallDepth / wallDepth);
                    wall.transform.localScale = scale;
                }

                wall.name = $"{prefabToUse.name}_{borderName}_{i}";

                if (parent != null)
                {
                    wall.transform.SetParent(parent.transform);
                }

                Undo.RegisterCreatedObjectUndo(wall, $"Create {borderName} Border Wall");
                count++;
            }
        }

        return count;
    }
}
