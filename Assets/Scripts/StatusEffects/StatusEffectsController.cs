using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatusEffectsController : MonoBehaviour
{
    public static StatusEffectsController instance;

    // Controllers, Managers
    private FreeplayController gameController;
    private GameDataManager gameDataManager;
    
    // Status effects
    List<StatusEffect> statusEffects = new List<StatusEffect>();
    public System.Collections.Generic.List<StatusEffect> StatusEffects
    {
        get { return statusEffects; }
        set { statusEffects = value; }
    }

    // Events, Delegates
    public delegate void StatusEffectsChangedAction();
    public event StatusEffectsChangedAction OnStatusEffectsChanged;
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
        gameDataManager.OnNewDayStarted += ExecuteDailyStatusEffects;
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
            if (statusEffect.Freqency == frequency)
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

    public void ExecuteOneShotStatusEffect(StatusEffect statusEffect)
    {
        if (statusEffect.Freqency == StatusEffectFrequency.OneShot)
        {
            ExecuteStatusEffect(statusEffect);
        } else
        {
            print("Status effect is not one shot");
        }
    }

    public void AddStatusEffects(List<StatusEffect> statusEffects)
    {
        this.StatusEffects.AddRange(statusEffects);
        OnStatusEffectsChanged();
    }

    public void AddStatusEffects(StatusEffect statusEffect)
    {
        this.StatusEffects.Add(statusEffect);
        OnStatusEffectsChanged();
    }

    public void RemoveStatusEffects(List<StatusEffect> statusEffects)
    {
        this.StatusEffects.RemoveAll(statusEffect => statusEffects.Contains(statusEffect));
        OnStatusEffectsChanged();
    }

    public void RemoveStatusEffects(StatusEffect statusEffect)
    {
        this.StatusEffects.Remove(statusEffect);
        OnStatusEffectsChanged();
    }


}
