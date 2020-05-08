using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatusEffectsManager : MonoBehaviour
{
    public static StatusEffectsManager instance;

    // Controllers, Managers
    private GameDataManager gameDataManager;

    // Status effects
    ObservableCollection<StatusEffect> statusEffects = new ObservableCollection<StatusEffect>();
    public ObservableCollection<StatusEffect> StatusEffects
    {
        get { return statusEffects; }
        set { statusEffects = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        UpdateReferences();
    }

    public void UpdateReferences()
    {

    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        gameDataManager = GameDataManager.instance;
        // Listen to game controller ticks and execute appropriate modifiers
        gameDataManager.OnDayStarted += ExecuteDailyStatusEffects;
        gameDataManager.OnNewWeekStarted += ExecuteWeeklyStatusEffects;
        gameDataManager.OnNewMonthStarted += ExecuteMonthlyStatusEffects;
        gameDataManager.OnNewYearStarted += ExecuteYearlyStatusEffects;
        SceneManager.sceneLoaded += SceneLoadedHandling;
    }

    private void SceneLoadedHandling(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log(this.GetType().ToString() + "scene loaded handled");

    }

    private void ExecuteDailyStatusEffects()
    {
        ExecuteStatusEffects(StatusEffectFrequency.Daily);
    }
    private void ExecuteWeeklyStatusEffects()
    {
        ExecuteStatusEffects(StatusEffectFrequency.Weekly);
    }
    private void ExecuteMonthlyStatusEffects()
    {
        ExecuteStatusEffects(StatusEffectFrequency.Monthly);
    }
    private void ExecuteYearlyStatusEffects()
    {
        ExecuteStatusEffects(StatusEffectFrequency.Yearly);
    }

    private void ExecuteStatusEffects(StatusEffectFrequency frequency)
    {
        foreach (StatusEffect statusEffect in StatusEffects)
        {
            if (statusEffect.Frequency == frequency)
            {
                ExecuteStatusEffect(statusEffect);
            }
        }
    }


    private void ExecuteStatusEffect(StatusEffect statusEffect)
    {
        switch (statusEffect.Type)
        {
            case StatusEffectType.Money:
                gameDataManager.Money += statusEffect.Value;
                break;
            case StatusEffectType.Mood:
                gameDataManager.Mood += statusEffect.Value;
                break;
            default:
                break;
        }
    }

    private void ExecuteOneShot(StatusEffect statusEffect)
    {
        switch (statusEffect.Type)
        {
            case StatusEffectType.Money:
                gameDataManager.Money += statusEffect.Value;
                break;
            case StatusEffectType.Mood:
                gameDataManager.Mood += statusEffect.Value;
                break;
            default:
                break;
        }
    }

    public void ApplyStatusEffects(List<StatusEffect> statusEffects)
    {
        foreach (StatusEffect statusEffect in statusEffects)
        {
            if (statusEffect.Frequency == StatusEffectFrequency.OneShot)
            {
                ExecuteOneShot(statusEffect);
            } else
            {
                StatusEffects.Add(statusEffect);
            }
        }
    }

    public void ApplyStatusEffects(StatusEffect statusEffect)
    {
        if (statusEffect.Frequency == StatusEffectFrequency.OneShot)
        {
            ExecuteOneShot(statusEffect);
        }
        else
        {
            StatusEffects.Add(statusEffect);
        }
    }

    public void RemoveStatusEffects(List<StatusEffect> statusEffects)
    {
        for (int i = statusEffects.Count - 1; i >= 0; i--)
        {
            RemoveStatusEffects(statusEffects[i]);
        }
    }

    public void RemoveStatusEffects(IEnumerable<StatusEffect> statusEffects)
    {
        for (int i = statusEffects.Count() - 1; i >= 0; i--)
        {
            RemoveStatusEffects(statusEffects.ElementAt(i));
        }
    }

    public void RemoveStatusEffects(StatusEffect statusEffect)
    {
        this.StatusEffects.Remove(statusEffect);
    }

    public void RemoveStatusEffects(StatusEffectFlags statusEffectFlags)
    {
        IEnumerable<StatusEffect> statusEffectsToDestoy = StatusEffects.Where(se => se.Flags.HasFlag(statusEffectFlags));
        RemoveStatusEffects(statusEffectsToDestoy);
    }


}
