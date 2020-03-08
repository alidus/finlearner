using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    // Singleton managers
    private GameManager gameManager;
    private GameDataManager gameDataManager;

    public Text moneyText;
    public Text moodText;
    public Image DayProgressBarFill;

    float timeSinceDayStart;
    // Modifiers
    private ModifiersContainer modifiersPool = new ModifiersContainer();


    // Jobs
    private List<Job> jobsPool = new List<Job>();
    private List<Job> activeJobs = new List<Job>();

    // Fin ops
    private List<Credit> credits = new List<Credit>();

    // Store catalogs
    public StoreCatalog homeStoreCatalog;

    // Store managers
    private Store homeStoreComponent;

    // UI references
    public GameObject homeStorePanel;
    public GameObject homeStoreCategoriesPanel;
    public GameObject homeStoreGridPanel;

    // Prefabs
    public GameObject homeStoreItemPrefab;
    public GameObject homeStoreCategoryButtonPrefab;

    // Cashing
    public Dictionary<ItemType, StoreItem> selectedObjectPerItemType;

    // Events
    public delegate void AddModifierAction(Modifier modifier);
    public event AddModifierAction OnModifierAdded;

    private void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;

        
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        gameDataManager = GameDataManager.Instance;
        homeStoreCatalog = Instantiate(homeStoreCatalog) as StoreCatalog;
        homeStoreCatalog.Init();
        // Create store (can be multiple) components
        homeStoreComponent = StoreComponentFactory.CreateStoreComponent(this.transform.gameObject, homeStoreCatalog, homeStorePanel, homeStoreItemPrefab, homeStoreCategoryButtonPrefab, homeStoreCategoriesPanel, homeStoreGridPanel);
        homeStoreComponent.OnInventoryItemClicked += HomeStoreItemClick;
        GameManager.OnStoreButtonPress += homeStoreComponent.Toggle;

        gameDataManager.Money = gameManager.GameMode.money;
        gameDataManager.Mood = gameManager.GameMode.mood;
        gameDataManager.Age = gameManager.GameMode.age;

        foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
        {
            //TODO: Add selectable items to list
        }

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        DayProgressBarFill.fillAmount = timeSinceDayStart / gameManager.GameMode.dayDuration;
        timeSinceDayStart += Time.deltaTime;
        if (timeSinceDayStart > gameManager.GameMode.dayDuration)
        {
            TickDay();
            timeSinceDayStart = 0;

            if (gameDataManager.DayCount - gameDataManager.Age * 365 > 365)
            {
                gameDataManager.Age += 1;
            }
            gameManager.UpdateInfoPanel();
        }
    }

    private void Init()
    {
        InitJobs();


        gameManager.UpdateUI();
    }

    private void InitJobs()
    {
        foreach (UnityEngine.Object job in Resources.LoadAll("ScriptableObjects/Jobs"))
        {
            jobsPool.Add(new Job((JobSOTemplate)job));
            print(job.name);
        }

        // DEBUG
        ActivateJob(jobsPool[0]);
    }



    private void ActivateJob(Job job)
    {
        activeJobs.Add(job);
        AppendModifiers(job.modifiers);
    }

    private void DeactivateJob(Job job)
    {
        activeJobs.Remove(job);
    }

    private void TickDay()
    {
        bool isEndOfWeek = false;
        
        gameDataManager.AddDayOfWeek();
        if (gameDataManager.DayOfWeekIndex == 0)
        {
            TickWeek();
            isEndOfWeek = true;
        }
        ApplyAllGlobalMultipliers(true);
        gameManager.AddDayToWeekProgressIndicator();
        gameDataManager.AddToDayCounter();
        
    }

    private void TickWeek()
    {

    }


    private void ApplyAllGlobalMultipliers(bool daily = true, bool weekly = false, bool yearly = false)
    {
        foreach (Modifier modifier in modifiersPool)
        {
            if (yearly && modifier.Freqency == ModifierEffectFreqency.Yearly)
            {
                // Apply yearly modifier
                ApplyModifier(modifier);
            } else if (weekly && modifier.Freqency == ModifierEffectFreqency.Weekly)
            {
                // Apply weekly modifier
                ApplyModifier(modifier);
            } else if (daily && (modifier.Freqency == ModifierEffectFreqency.Daily))
            {
                // Apply daily modifier
                ApplyModifier(modifier);
            }
        }
    }

    /// <summary>
    /// Apply modifier effects instantly
    /// </summary>
    /// <param name="modifier"></param>
    private void ApplyModifier(Modifier modifier)
    {
        switch (modifier.Type)
        {
            case ModifierType.Money:
                AddMoney(modifier.Value);
                break;
            case ModifierType.Mood:
                AddMood(modifier.Value);
                break;
            default:
                break;
        }
    }

    private void AppendModifiers(List<Modifier> modifiers)
    {
        this.modifiersPool.AddRange(modifiers);
        foreach(Modifier modifier in modifiers)
        {
            OnModifierAdded(modifier);
        }
    }

    private void AppendModifiers(Modifier modifier)
    {
        this.modifiersPool.Add(modifier);
        OnModifierAdded(modifier);
    }



    private void OnGameStarted(GameMode gameMode)
    {

    }

    public void AddMoney(int value)
    {
        gameDataManager.Money += value;
        gameManager.UpdateMoneyPanel();
    }

    public void AddMood(int value)
    {
        gameDataManager.Mood += value;
        gameManager.UpdateMoodPanel();
    }

    public void HomeStoreItemClick(StoreItem item)
    {
        if (!item.IsOwned)
        {
            // Try to buy
            if (item.Price <= gameDataManager.Money)
            {
                item.IsOwned = true;
                AddMoney(-item.Price);
                foreach (Modifier modifier in item.Modifiers)
                {
                    if (modifier.Freqency == ModifierEffectFreqency.OneShot)
                    {
                        ApplyModifier(modifier);
                    } else
                    {
                        AppendModifiers(modifier);
                    }
                }
                
            }
        } else
        {
            // Equip
            if (item.EquipBehavour != null)
            {
                item.EquipBehavour.Equip();
                homeStoreComponent.storeCatalog.EquipItem(item);
                gameManager.UpdateFlatAppearance();
            }
            
        }
        
    }

}
