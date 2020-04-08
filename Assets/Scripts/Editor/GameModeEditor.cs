using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

enum EditorGameConditionType { ValueCondition, PurchaseCondition };


[CustomEditor(typeof(GameMode), true)]
public class GameModeEditor : Editor
{
    EditorGameConditionType WinConditionAdditionType;
    EditorGameConditionType LoseConditionAdditionType;
    GameMode gameMode;

    private void OnEnable()
    {
        gameMode = target as GameMode;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUIUtility.labelWidth = 50;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("title"));
        EditorGUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 45;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("age"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("money"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("mood"));
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(50);

        gameMode.WinConditionsContainer = DrawConditionsGroup("Win conditions", GameConditionGroup.Win, gameMode.WinConditionsContainer);

        GUILayout.Space(50);
        // Lose conditions array

        gameMode.LoseConditionsContainer = DrawConditionsGroup("Lose conditions", GameConditionGroup.Lose, gameMode.LoseConditionsContainer);

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    void DrawValueCondition(GameCondition condition)
    {
        ValueCondition valueCondition = (ValueCondition)condition;
        EditorGUILayout.LabelField("Value condition");
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        valueCondition.TargetVariable = (TargetVariable)EditorGUILayout.EnumPopup(valueCondition.TargetVariable);
        valueCondition.ValueConditionOperator = (ValueConditionOperator)EditorGUILayout.EnumPopup(valueCondition.ValueConditionOperator);
        valueCondition.TargetFloatValue = EditorGUILayout.FloatField(valueCondition.TargetFloatValue);
        GUILayout.EndHorizontal();
    }

    GameConditionsContainer DrawConditionsGroup(string title, GameConditionGroup conditionGroup, GameConditionsContainer gameConditionsContainer)
    {
        EditorGUILayout.LabelField(title + " (Size " + gameConditionsContainer.Count.ToString() + "):");

        EditorGUILayout.BeginHorizontal();
        EditorGameConditionType gameConditionType;
        switch (conditionGroup)
        {
            case GameConditionGroup.Win:
                WinConditionAdditionType = (EditorGameConditionType)EditorGUILayout.EnumPopup(WinConditionAdditionType);
                gameConditionType = WinConditionAdditionType;
                break;
            case GameConditionGroup.Lose:
                LoseConditionAdditionType = (EditorGameConditionType)EditorGUILayout.EnumPopup(LoseConditionAdditionType);
                gameConditionType = LoseConditionAdditionType;
                break;
            default:
                WinConditionAdditionType = (EditorGameConditionType)EditorGUILayout.EnumPopup(WinConditionAdditionType);
                gameConditionType = WinConditionAdditionType;
                break;
        }
        if (GUILayout.Button("Add"))
        {
            switch (gameConditionType)
            {
                case EditorGameConditionType.ValueCondition:
                    Undo.RecordObject(gameMode, "Added win condition");
                    gameMode.AddCondition(new ValueCondition(GameDataManager.instance), conditionGroup);
                    break;
                default:
                    break;
            }
        }
        EditorGUILayout.EndHorizontal();

        // Goes backward to be able to remove elements without unexpected behavior
        for (int i = gameConditionsContainer.ValueConditions.Count - 1; i >= 0; i--)
        {
            ValueCondition valueCondition = gameConditionsContainer.ValueConditions[i];
            EditorGUILayout.LabelField("______________________________________________");
            DrawValueCondition(valueCondition);
            EditorGUILayout.LabelField("______________________________________________");
            GUILayout.Space(20);
            if (GUILayout.Button("Remove"))
            {
                Undo.RecordObject(gameMode, "Removed win condition");
                gameMode.RemoveCondition(valueCondition, conditionGroup);
            }
        }

        return gameConditionsContainer;
    }
}


















