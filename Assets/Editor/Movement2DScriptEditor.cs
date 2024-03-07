using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Movement2DScript))]
public class Movement2DScriptEditor : Editor
{
    private SerializedProperty movementSpeedProp;
    private SerializedProperty controlledByAIProp;
    private SerializedProperty chanceOfMovementAwakeProp;
    private SerializedProperty chanceOfMovementAwakeningProp;
    private SerializedProperty chanceOfMovementSleepProp;
    private SerializedProperty playerNumberProp;
    private SerializedProperty soundEffectProp;

    private void OnEnable()
    {
        movementSpeedProp = serializedObject.FindProperty("movementSpeed");
        controlledByAIProp = serializedObject.FindProperty("controlledByAI");
        chanceOfMovementAwakeProp = serializedObject.FindProperty("chanceOfMovementAwake");
        chanceOfMovementAwakeningProp = serializedObject.FindProperty("chanceOfMovementAwakening");
        chanceOfMovementSleepProp = serializedObject.FindProperty("chanceOfMovementSleep");
        playerNumberProp = serializedObject.FindProperty("playerNumber");
        soundEffectProp = serializedObject.FindProperty("soundEffect");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw default inspector property fields
        EditorGUILayout.PropertyField(movementSpeedProp);
        EditorGUILayout.PropertyField(controlledByAIProp);
        if (controlledByAIProp.boolValue)
        {
            EditorGUILayout.PropertyField(chanceOfMovementAwakeProp);
            EditorGUILayout.PropertyField(chanceOfMovementAwakeningProp);
            EditorGUILayout.PropertyField(chanceOfMovementSleepProp);
        } 
        EditorGUILayout.PropertyField(playerNumberProp);
        EditorGUILayout.PropertyField(soundEffectProp);

        serializedObject.ApplyModifiedProperties();
    }
}
