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

        if (job != null)
        {
            InitEdtiorSalaryValues(job);

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

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Time consumption: ", GUILayout.MaxWidth(80));
            Undo.RecordObject(job, "Edit job time consumption");
            job.HoursOfWeekToConsume = EditorGUILayout.FloatField(job.HoursOfWeekToConsume);
            GUILayout.EndHorizontal();

            salariesFoldout = EditorGUILayout.Foldout(salariesFoldout, "Salary");
            if (salariesFoldout)
            {
                DrawSalaryField(ref dailySalary, "Daily: ", "Ежедневная зарплата", StatusEffectFrequency.Daily);
                DrawSalaryField(ref weeklySalary, "Weekly: ", "Еженедельная зарплата", StatusEffectFrequency.Weekly);
                DrawSalaryField(ref monthlySalary, "Montly: ", "Ежемесячная зарплата", StatusEffectFrequency.Monthly);
                DrawSalaryField(ref yearlySalary, "Yearly: ", "Ежегодная зарплата", StatusEffectFrequency.Yearly);
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
        dailySalary = job.StatusEffects.Find((se) => se != null && se.Flags.HasFlag(StatusEffectFlags.Job) && se.Frequency == StatusEffectFrequency.Daily)?.Value ?? 0;
        weeklySalary = job.StatusEffects.Find((se) => se != null && se.Flags.HasFlag(StatusEffectFlags.Job) && se.Frequency == StatusEffectFrequency.Weekly)?.Value ?? 0;
        monthlySalary = job.StatusEffects.Find((se) => se != null && se.Flags.HasFlag(StatusEffectFlags.Job) && se.Frequency == StatusEffectFrequency.Monthly)?.Value ?? 0;
        yearlySalary = job.StatusEffects.Find((se) => se != null && se.Flags.HasFlag(StatusEffectFlags.Job) && se.Frequency == StatusEffectFrequency.Yearly)?.Value ?? 0;
    }

    void DrawSalaryField(ref float editorSalaryVariable, string labelText, string seTitlePrefix, StatusEffectFrequency frequency)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(labelText, GUILayout.MaxWidth(80));
        editorSalaryVariable = EditorGUILayout.FloatField(editorSalaryVariable);
        var targetSalarySEs = job.StatusEffects.FindAll((se) => se != null && se.Flags.HasFlag(StatusEffectFlags.Job) && se.Frequency == frequency);
        // If there are no daily salary SE
        if (targetSalarySEs.Count == 0)
        {
            // If daily salary is set in editor -> create appropriate SE for job
            if (editorSalaryVariable != 0)
            {
                job.StatusEffects.Add(new StatusEffect(seTitlePrefix + " (" + job.Title + ")", editorSalaryVariable, StatusEffectType.Money, frequency, StatusEffectFlags.Job));
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
