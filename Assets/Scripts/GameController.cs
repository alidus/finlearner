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

    // Modifiers related
    private List<IncomeModifier> incomeModifiers = new List<IncomeModifier>();
    private List<MoodModifier> moodModifiers = new List<MoodModifier>();

    private List<Job> jobsPool = new List<Job>();
    private List<Job> activeJobs = new List<Job>();

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

        //DEBUG
        AddModifier(new IncomeModifier("Выплаты за ипотеку", -200, ModifierType.Weekly));
        AddModifier(new MoodModifier("Выплаты за ипотеку", -3, ModifierType.Weekly));

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
        AddModifier(job.incomeModifier);
        AddModifier(job.moodModifier);
    }

    private void DeactivateJob(Job job)
    {
        activeJobs.Remove(job);
        RemoveIncomeModifier(job.incomeModifier);
        RemoveMoodModifier(job.moodModifier);
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
        int deltaMoney = GetModifiersCalculation(incomeModifiers.ConvertAll(x => (Modifier)x), true, isEndOfWeek);
        int deltaMood = GetModifiersCalculation(moodModifiers.ConvertAll(x => (Modifier)x), true, isEndOfWeek);
        gameManager.AddDayToWeekProgressIndicator();
        AddMoney(deltaMoney);
        AddMood(deltaMood);
        gameDataManager.DailyIncome = deltaMoney;
        gameDataManager.DailyMoodChange = deltaMood;
        gameDataManager.AddToDayCounter();
        
    }

    private void TickWeek()
    {

    }

    private int GetModifiersCalculation(List<Modifier> modifiers, bool countDaily, bool countWeekly)
    {
        int result = 0;
        foreach (Modifier mod in modifiers)
        {
            if (countDaily && mod.type == ModifierType.Daily || countWeekly && mod.type == ModifierType.Weekly)
            {
                result += mod.value;
            }
            
        }
        return result;
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
                switch (item.MoodModifier.type)
                {
                    case ModifierType.Daily:
                        AddModifier(item.MoodModifier);
                        break;
                    case ModifierType.Weekly:
                        AddModifier(item.MoodModifier);
                        break;
                    case ModifierType.OneShot:
                        AddMood(item.MoodModifier.value);
                        break;
                    default:
                        break;
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

    public void AddModifier(IncomeModifier mod)
    {
        incomeModifiers.Add(mod);
        gameManager.AddModifierPanel(mod);
    }

    public void AddModifier(MoodModifier mod)
    {
        moodModifiers.Add(mod);
        gameManager.AddModifierPanel(mod);
    }

    public void RemoveIncomeModifier(IncomeModifier mod)
    {
        incomeModifiers.Remove(mod);
    }

    public void RemoveIncomeModifier(int index)
    {
        incomeModifiers.RemoveAt(index);
    }

    public void RemoveMoodModifier(MoodModifier mod)
    {
        moodModifiers.Remove(mod);
    }

    public void RemoveMoodModifier(int index)
    {
        moodModifiers.RemoveAt(index);
    }

}
