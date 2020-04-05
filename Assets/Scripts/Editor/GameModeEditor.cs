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

    SerializedProperty winConditions;
    SerializedProperty loseConditions;


    private void OnEnable()
    {
        gameMode = (GameMode)target;
        winConditions = serializedObject.FindProperty("winConditions");
        loseConditions = serializedObject.FindProperty("loseConditions");
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

        gameMode.WinConditions = DrawConditionsGroup("Win conditions", GameConditionGroup.Win, gameMode.WinConditions);

        GUILayout.Space(50);
        // Lose conditions array

        gameMode.LoseConditions = DrawConditionsGroup("Lose conditions", GameConditionGroup.Lose, gameMode.LoseConditions);

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

    List<GameCondition> DrawConditionsGroup(string title, GameConditionGroup group, List<GameCondition> gameConditions)
    {
        EditorGUILayout.LabelField(title + " (Size " + gameConditions.Count.ToString() + "):");

        EditorGUILayout.BeginHorizontal();
        EditorGameConditionType gameConditionType;
        switch (group)
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
                    gameMode.AddCondition(new ValueCondition(GameDataManager.instance), group);
                    break;
                default:
                    break;
            }
        }
        EditorGUILayout.EndHorizontal();

        // Goes backward to be able to remove elements without unexpected behavior
        for (int i = gameConditions.Count - 1; i >= 0; i--)
        {
            GameCondition condition = gameConditions[i];

            EditorGUILayout.LabelField("______________________________________________");
            if (condition is ValueCondition)
            {
                DrawValueCondition(condition);
            }

            GUILayout.Space(20);
            if (GUILayout.Button("Remove"))
            {
                gameMode.RemoveCondition(condition, group);
            }

            EditorGUILayout.LabelField("______________________________________________");
        }

        return gameConditions;
    }
}


















