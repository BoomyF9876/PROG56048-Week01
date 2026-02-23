using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor for the PCG (Procedural Content Generation) component.
/// Provides additional functionality in the Unity Inspector for managing procedural map generation.
/// </summary>
[CustomEditor(typeof(PCG))]
public class PCGEditor : Editor
{
    #region Serialized Properties

    private SerializedProperty tilesProperty;

    #endregion

    #region Unity Editor Callbacks

    private void OnEnable()
    {
        // Cache serialized properties for validation
        tilesProperty = serializedObject.FindProperty("tiles");
    }

    /// <summary>
    /// Overrides the default inspector GUI to add custom functionality.
    /// </summary>
    public override void OnInspectorGUI()
    {
        // Draw the default inspector options
        base.OnInspectorGUI();

        // Reference to the PCG script attached to the selected GameObject
        PCG pcg = (PCG)target;

        EditorGUILayout.Space(10);

        // Show validation warnings
        DrawValidationMessages();

        EditorGUILayout.Space(5);

        // Show play mode indicator
        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox(
                "Map regeneration requires Play Mode. Enter Play Mode to test generation.",
                MessageType.Info
            );
        }

        // Disable the button in edit mode
        EditorGUI.BeginDisabledGroup(!Application.isPlaying);
        {
            if (GUILayout.Button("Regenerate Map", GUILayout.Height(30)))
            {
                // Register for undo
                Undo.RecordObject(pcg, "Regenerate Map");

                pcg.RegenerateMap();
            }
        }
        EditorGUI.EndDisabledGroup();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Draws validation messages for common configuration issues.
    /// </summary>
    private void DrawValidationMessages()
    {
        serializedObject.Update();

        // Check if tiles list is empty
        if (tilesProperty == null || tilesProperty.arraySize == 0)
        {
            EditorGUILayout.HelpBox(
                "No tiles configured! Add at least one tile to the Tiles list.",
                MessageType.Warning
            );
            return;
        }

        // Check for null tile references
        bool hasNullTile = false;
        for (int i = 0; i < tilesProperty.arraySize; i++)
        {
            SerializedProperty tileElement = tilesProperty.GetArrayElementAtIndex(i);
            SerializedProperty tileRef = tileElement.FindPropertyRelative("tile");

            if (tileRef.objectReferenceValue == null)
            {
                hasNullTile = true;
                break;
            }
        }

        if (hasNullTile)
        {
            EditorGUILayout.HelpBox(
                "One or more tiles have missing TileSO references!",
                MessageType.Error
            );
        }
    }

    #endregion
}
