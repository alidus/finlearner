using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class JobEditorWindow : EditorWindow
{

    Job job;
    float dailySalary, weeklySalary, monthlySalary, yearlySalary;
    string newJobAssetName;
    bool salariesFoldout;


    [MenuItem("GameEditors/Job/JobEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(JobEditorWindow));
    }

    private void OnEnable()
    {
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("New job asset name: ", GUILayout.MaxWidth(80));
        newJobAssetName = EditorGUILayout.TextField(newJobAssetName);
        GUILayout.EndHorizontal();
        if (GUILayout.Button("New job", GUILayout.Width(100), GUILayout.Height(40)))
        {
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Job>(), AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/ScriptableObjects/JobExchange/Jobs/" + newJobAssetName + ".asset"));
            job = AssetDatabase.LoadMainAssetAtPath("Assets/Resources/ScriptableObjects/JobExchange/Jobs/" + newJobAssetName + ".asset") as Job;
        }

        job = EditorGUILayout.ObjectField(job, typeof(Job), true) as Job;
        InitEdtiorSalaryValues(job);

        if (job != null)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Title: ", GUILayout.MaxWidth(80));
            Undo.RecordObject(job, "Edit job title");
            job.Title = EditorGUILayout.TextField(job.Title);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Title: ", GUILayout.MaxWidth(80));
            Undo.RecordObject(job, "Edit job title");
            job.Category = (JobCategory)EditorGUILayout.ObjectField(job.Category, typeof(JobCategory), allowSceneObjects: true);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Description: ", GUILayout.MaxWidth(80));
            Undo.RecordObject(job, "Edit job description");
            job.Description = EditorGUILayout.TextField(job.Description);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Sprite: ", GUILayout.MaxWidth(80));
            Undo.RecordObject(job, "Edit job sprite");
            job.Sprite = (Sprite)EditorGUILayout.ObjectField(job.Sprite, typeof(Sprite), allowSceneObjects: true);
            GUILayout.EndHorizontal();

            salariesFoldout = EditorGUILayout.Foldout(salariesFoldout, "Salary");
            if (salariesFoldout)
            {
                DrawSalaryField(ref dailySalary, "Daily: ", "Ежедневная зарплата", StatusEffectFlags.DailySalary);
                DrawSalaryField(ref weeklySalary, "Weekly: ", "Еженедельная зарплата", StatusEffectFlags.WeeklySalary);
                DrawSalaryField(ref monthlySalary, "Montly: ", "Ежемесячная зарплата", StatusEffectFlags.MonthlySalary);
                DrawSalaryField(ref yearlySalary, "Yearly: ", "Ежегодная зарплата", StatusEffectFlags.YearlySalary);
            }

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Is available: ", GUILayout.MaxWidth(80));
            Undo.RecordObject(job, "Change job availability");
            job.CanBeEquipped = EditorGUILayout.Toggle(job.CanBeEquipped);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Is active: ", GUILayout.MaxWidth(80));
            Undo.RecordObject(job, "Change job availability");
            job.IsEquipped = EditorGUILayout.Toggle(job.IsEquipped);
            GUILayout.EndHorizontal();
        }

        if (GUI.changed)
        {
            if (job)
            {
                EditorUtility.SetDirty(job);
            }
        }
    }

    void InitEdtiorSalaryValues(Job job)
    {
        dailySalary = job.StatusEffects.Find((se) => se.Flags.HasFlag(StatusEffectFlags.DailySalary))?.Value ?? 0;
        weeklySalary = job.StatusEffects.Find((se) => se.Flags.HasFlag(StatusEffectFlags.WeeklySalary))?.Value ?? 0;
        monthlySalary = job.StatusEffects.Find((se) => se.Flags.HasFlag(StatusEffectFlags.MonthlySalary))?.Value ?? 0;
        yearlySalary = job.StatusEffects.Find((se) => se.Flags.HasFlag(StatusEffectFlags.YearlySalary))?.Value ?? 0;
    }

    void DrawSalaryField(ref float editorSalaryVariable, string labelText, string seTitlePrefix, StatusEffectFlags targetFlag)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(labelText, GUILayout.MaxWidth(80));
        editorSalaryVariable = EditorGUILayout.FloatField(editorSalaryVariable);
        var targetSalarySEs = job.StatusEffects.FindAll((se) => se.Flags.HasFlag(targetFlag));
        // If there are no daily salary SE
        if (targetSalarySEs.Count == 0)
        {
            // If daily salary is set in editor -> create appropriate SE for job
            if (editorSalaryVariable != 0)
            {
                StatusEffectFrequency frequency;
                switch (targetFlag)
                {
                    case StatusEffectFlags.DailySalary:
                        frequency = StatusEffectFrequency.Daily;
                        break;
                    case StatusEffectFlags.WeeklySalary:
                        frequency = StatusEffectFrequency.Weekly;
                        break;
                    case StatusEffectFlags.MonthlySalary:
                        frequency = StatusEffectFrequency.Monthly;
                        break;
                    case StatusEffectFlags.YearlySalary:
                        frequency = StatusEffectFrequency.Yearly;
                        break;
                    default:
                        frequency = StatusEffectFrequency.Daily;
                        break;
                }
                job.StatusEffects.Add(new StatusEffect(seTitlePrefix + " (" + job.Title + ")", editorSalaryVariable, StatusEffectType.Money, frequency, targetFlag));
            }
        }
        else
        // Edit existing daily salary SE
        if (targetSalarySEs.Count == 1)
        {
            targetSalarySEs[0].Title = seTitlePrefix + " (" + job.Title + ")";
            if (targetSalarySEs[0].Value == 0)
            {
                job.StatusEffects.Remove(targetSalarySEs[0]);
            }
            else
            if (targetSalarySEs[0].Value != editorSalaryVariable)
            {
                targetSalarySEs[0].Value = editorSalaryVariable;
            }
        }
        GUILayout.EndHorizontal();
    }
}
