using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectsController : MonoBehaviour
{
    public static StatusEffectsController instance;

    // Controllers, Managers
    private GameController gameController;
    private GameDataManager gameDataManager;

    // UI elements
    private GameObject statusEffectsPanel;

    // Prefabs
    public GameObject StatusEffectPanelPrefab;

    // Status effects
    List<StatusEffect> statusEffects = new List<StatusEffect>();

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
        statusEffectsPanel = GameObject.Find("StatusEffectsPanel");
        var z = 5;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        gameController = GameController.instance;
        gameDataManager = GameDataManager.instance;
        // Listen to game controller ticks and execute appropriate modifiers
        gameController.OnDailyTick += ExecuteDailyStatusEffects;
        gameController.OnWeeklyTick += ExecuteWeeklyStatusEffects;
        gameController.OnMonthlyTick += ExecuteMonthlyStatusEffects;
        gameController.OnYearlyTick += ExecuteYearlyStatusEffects;
    }

    public GameObject CreateStatusEffectPanel(StatusEffect statusEffect)
    {
        if (statusEffectsPanel)
        {
            Transform[] children = statusEffectsPanel.transform.GetComponentsInChildren<Transform>();
            GameObject panel = Instantiate(StatusEffectPanelPrefab);
            string ContentPanelName = "";
            switch (statusEffect.Type)
            {
                case StatusEffectType.Money:
                    ContentPanelName = "MoneyStatusEffectsContent";
                    break;
                case StatusEffectType.Mood:
                    ContentPanelName = "MoodStatusEffectsContent";
                    break;
                default:
                    break;
            }

            // Iterate through children of modifiers panel and find content panels of each mod type containing array of modifiers
            foreach (Transform child in children)
            {
                if (child.name == ContentPanelName)
                {
                    panel.transform.SetParent(child);
                    panel.transform.localScale = new Vector3(1, 1, 1);
                    panel.transform.Find("Info").transform.Find("TopPanel").transform.Find("TitleText").GetComponent<Text>().text = statusEffect.Name;
                    Transform BottomPanelTransform = panel.transform.Find("Info").transform.Find("BottomPanel");
                    BottomPanelTransform.transform.Find("TypePanel").GetComponentInChildren<Text>().text = statusEffect.Freqency.ToString();
                    BottomPanelTransform.transform.Find("ValuePanel").GetComponentInChildren<Text>().text = (statusEffect.Value > 0 ? "+" : "") + statusEffect.Value;
                }
            }

            return panel;
        } else
        {
            return null;
        }
    }

    private void ExecuteDailyStatusEffects()
    {
        ApplyStatusEffects(StatusEffectFrequency.Daily);
    }
    private void ExecuteWeeklyStatusEffects()
    {
        ApplyStatusEffects(StatusEffectFrequency.Weekly);
    }
    private void ExecuteMonthlyStatusEffects()
    {
        ApplyStatusEffects(StatusEffectFrequency.Monthly);
    }
    private void ExecuteYearlyStatusEffects()
    {
        ApplyStatusEffects(StatusEffectFrequency.Yearly);
    }

    private void ApplyStatusEffects(StatusEffectFrequency frequency)
    {
        foreach (StatusEffect statusEffect in statusEffects)
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
        this.statusEffects.AddRange(statusEffects);
        foreach (StatusEffect statusEffect in statusEffects)
        {
            CreateStatusEffectPanel(statusEffect);
        }
    }

    public void AddStatusEffects(StatusEffect statusEffect)
    {
        this.statusEffects.Add(statusEffect);
        CreateStatusEffectPanel(statusEffect);
    }
}
