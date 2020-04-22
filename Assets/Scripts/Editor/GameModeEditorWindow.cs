using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public enum EditorGameConditionType { ValueCondition, OwnCondition }

public class GameModeEditorWindow : EditorWindow
{
    GameMode gameMode;
    GUIStyle conditionGroupTitleStyle = new GUIStyle();
    GUIStyle conditionLabelStyle = new GUIStyle();


    Vector2 scrollPos;
    private void OnEnable()
    {
        conditionGroupTitleStyle.fontSize = 20;
        conditionGroupTitleStyle.normal.textColor = Color.white;

        conditionLabelStyle.fontSize = 15;
        conditionLabelStyle.normal.textColor = Color.cyan;

    }

    [MenuItem("GameEditors/GameModeEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GameModeEditorWindow));
    }

    void OnGUI()
    {
        gameMode = EditorGUILayout.ObjectField(gameMode, typeof(GameMode), true) as GameMode;
        if (gameMode != null)
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.ExpandWidth(true), GUILayout.MinHeight(200), GUILayout.MaxHeight(1000), GUILayout.ExpandHeight(true));
            GUILayout.Space(5);
            EditorGUILayout.LabelField("Editing gameMode: " + gameMode.Title);
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Title", GUILayout.MaxWidth(40));
            Undo.RecordObject(gameMode, "Change title");
            gameMode.Title = EditorGUILayout.TextField(gameMode.Title);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Age", GUILayout.MaxWidth(30));
            gameMode.Age = EditorGUILayout.IntField(gameMode.Age);
            Undo.RecordObject(gameMode, "Change age");
            EditorGUILayout.LabelField("Money", GUILayout.MaxWidth(40));
            Undo.RecordObject(gameMode, "Change money");
            gameMode.Money = EditorGUILayout.FloatField(gameMode.Money);
            EditorGUILayout.LabelField("Mood", GUILayout.MaxWidth(40));
            Undo.RecordObject(gameMode, "Change mood");
            gameMode.Mood = EditorGUILayout.FloatField(gameMode.Mood);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Conditions group count: " + gameMode.GameConditionGroups.Count);
            if (GUILayout.Button("Remove all groups"))
            {
                Undo.RecordObject(gameMode, "Remove all groups");
                gameMode.ClearGameConditionGroups();
            }
            EditorGUILayout.EndHorizontal();

            for (int i = gameMode.GameConditionGroups.Count - 1; i >= 0; i--)
            {
                DrawConditionGroup(gameMode.GameConditionGroups[i]);
                GUILayout.Space(20);
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Create new group"))
            {
                Undo.RecordObject(gameMode, "Created new group");
                GameConditionsGroupFlags flags = 0;
                string title = "NEW_CONDITION_GROUP";
                if (gameMode.GetGameConditionGroupsWithFlags(GameConditionsGroupFlags.WinConditionsGroup).Count == 0)
                {
                    flags = GameConditionsGroupFlags.WinConditionsGroup;
                    title = "Winning condition group";
                } else if (gameMode.GetGameConditionGroupsWithFlags(GameConditionsGroupFlags.LoseConditionGroup).Count == 0)
                {
                    flags = GameConditionsGroupFlags.LoseConditionGroup;
                    title = "Losing condition group";
                }
                gameMode.AddConditionGroup(new GameConditionGroup(title, flags));
            }
            EditorGUILayout.EndScrollView();
        }
    }


    GameConditionGroup DrawConditionGroup(GameConditionGroup conditionGroup)
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        // Select group title color based on group flags
        switch (conditionGroup.Flags)
        {
            case GameConditionsGroupFlags.WinConditionsGroup:
                conditionGroupTitleStyle.normal.textColor = new Color(0.3f, 1, 0.3f);
                break;
            case GameConditionsGroupFlags.LoseConditionGroup:
                conditionGroupTitleStyle.normal.textColor = new Color(1, 0.3f, 0.3f);
                break;
            default:
                conditionGroupTitleStyle.normal.textColor = Color.white;
                break;
        }

        EditorGUILayout.LabelField("Condition group " + (gameMode.GameConditionGroups.IndexOf(conditionGroup) + 1).ToString() + ": " + conditionGroup.Title, conditionGroupTitleStyle, GUILayout.Height(35));
        if (GUILayout.Button("Remove group", GUILayout.ExpandWidth(false), GUILayout.Height(40)))
        {
            Undo.RecordObject(gameMode, "Remove group");
            gameMode.RemoveConditionGroup(conditionGroup);
        }
        GUILayout.EndHorizontal();
        conditionGroup.Title = EditorGUILayout.TextField(conditionGroup.Title);


        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Winning conditions?", GUILayout.ExpandWidth(false));
        if (EditorGUILayout.Toggle(conditionGroup.Flags.HasFlag(GameConditionsGroupFlags.WinConditionsGroup)))
        {
            if (!conditionGroup.Flags.HasFlag(GameConditionsGroupFlags.WinConditionsGroup))
            {
                Undo.RecordObject(gameMode, "Set win conditions flag");
                conditionGroup.Flags |= GameConditionsGroupFlags.WinConditionsGroup;
            }
        } else
        {
            if (conditionGroup.Flags.HasFlag(GameConditionsGroupFlags.WinConditionsGroup))
            {
                Undo.RecordObject(gameMode, "Set lose conditions flag");
                conditionGroup.Flags &= ~GameConditionsGroupFlags.WinConditionsGroup;
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Losing conditions?", GUILayout.ExpandWidth(false));
        if (EditorGUILayout.Toggle(conditionGroup.Flags.HasFlag(GameConditionsGroupFlags.LoseConditionGroup)))
        {
            if (!conditionGroup.Flags.HasFlag(GameConditionsGroupFlags.LoseConditionGroup))
                conditionGroup.Flags |= GameConditionsGroupFlags.LoseConditionGroup;
        }
        else
        {
            if (conditionGroup.Flags.HasFlag(GameConditionsGroupFlags.LoseConditionGroup))
                conditionGroup.Flags &= ~GameConditionsGroupFlags.LoseConditionGroup;
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        DrawListOfConditions(conditionGroup.GameConditionsContainer.ValueConditions, conditionGroup);
        DrawListOfConditions(conditionGroup.GameConditionsContainer.OwnConditions, conditionGroup);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Value condition", GUILayout.ExpandWidth(false)))
        {
            Undo.RecordObject(gameMode, "Added value condition");
            gameMode.AddCondition(new ValueCondition(), conditionGroup);
        }
        if (GUILayout.Button("Add Own condition", GUILayout.ExpandWidth(false)))
        {
            Undo.RecordObject(gameMode, "Added own condition");
            gameMode.AddCondition(new OwnCondition(null), conditionGroup);
        }
        EditorGUILayout.EndHorizontal();
        // Draw list of value conditions
        // Goes backward to be able to remove elements without unexpected behavior
        

        return conditionGroup;
    }

    void DrawListOfConditions<T>(List<T> conditions, GameConditionGroup conditionGroup) where T : GameCondition
    {
        for (int i = conditions.Count - 1; i >= 0; i--)
        {
            T condition = conditions[i];
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.BeginHorizontal();
            switch (condition.GetType().ToString())
            {
                case "ValueCondition":
                    conditionLabelStyle.normal.textColor = new Color(0.6f, 0.6f, 1);
                    break;
                case "OwnCondition":
                    conditionLabelStyle.normal.textColor = new Color(0.6f, 1, 0.6f);
                    break;
                default:
                    break;
            }
            EditorGUILayout.LabelField(condition.GetType().ToString(), conditionLabelStyle, GUILayout.Width(150));
            if (GUILayout.Button("Remove condition", GUILayout.ExpandWidth(false), GUILayout.Height(30)))
            {
                Undo.RecordObject(gameMode, "Removed win condition");
                gameMode.RemoveCondition(condition, conditionGroup);
            }
            GUILayout.EndHorizontal();
            if (typeof(T) == typeof(ValueCondition))
            {
                DrawValueCondition(condition as ValueCondition);
            }
            else if (typeof(T) == typeof(OwnCondition))
            {
                DrawOwnCondition(condition as OwnCondition);
            }
            GUILayout.Space(20);
            
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
    }

    void DrawValueCondition(ValueCondition valueCondition)
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        valueCondition.TargetVariable = (TargetVariable)EditorGUILayout.EnumPopup(valueCondition.TargetVariable);
        valueCondition.ValueConditionOperator = (ValueConditionOperator)EditorGUILayout.EnumPopup(valueCondition.ValueConditionOperator);
        valueCondition.TargetFloatValue = EditorGUILayout.FloatField(valueCondition.TargetFloatValue);
        GUILayout.EndHorizontal();
    }

    private void DrawOwnCondition(OwnCondition ownCondition)
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        ownCondition.TargetItem = EditorGUILayout.ObjectField(ownCondition.TargetItem, typeof(Item), true) as Item;
        GUILayout.EndHorizontal();
    }
}
