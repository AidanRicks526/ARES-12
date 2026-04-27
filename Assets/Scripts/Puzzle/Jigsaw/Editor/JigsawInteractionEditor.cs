using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JigsawBoard))]
public class JigsawBoardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        JigsawBoard board = (JigsawBoard)target;
        serializedObject.Update();

        // Draw everything up to totalPieces manually
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tilePrefab"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("puzzleImage"));

        // Randomize toggle
        EditorGUILayout.PropertyField(serializedObject.FindProperty("randomizePieceCount"));

        if (board.randomizePieceCount)
        {
            // Show min/max instead of fixed count
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("minPieces"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxPieces"));
            EditorGUI.indentLevel--;
        }
        else
        {
            // Show fixed piece count
            EditorGUILayout.PropertyField(serializedObject.FindProperty("totalPieces"));
        }

        // Draw the rest of the inspector normally
        EditorGUILayout.PropertyField(serializedObject.FindProperty("autoCalculate"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("manualColumns"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("manualRows"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maintainAspectRatio"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tileWidth"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tileHeight"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("curveResolution"));

        serializedObject.ApplyModifiedProperties();
    }
}