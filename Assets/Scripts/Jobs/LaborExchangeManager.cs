using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaborExchangeManager : MonoBehaviour
{
    public static LaborExchangeManager instance;
    private UIManager uiManager;
    private GameManager gameManager;
    private GameDataManager gameDataManager;
    private StatusEffectsController statusEffectsController;
    private HouseManager houseManager;

    // Events, Delegates
    public delegate void LaborExchangeStateChangedAction();
    public event LaborExchangeStateChangedAction OnLaborExchangeStateChanged;

    private List<Job> jobs = new List<Job>();
    public System.Collections.Generic.List<Job> Jobs
    {
        get { return jobs; }
        set { jobs = value; }
    }

    public JobCategory SelectedCategory { get; set; }
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
    
    public List<Job> GetAllJobsOfCategory(JobCategory category)
    {
        List<Job> result = new List<Job>();
        foreach (Job job in Jobs)
        {
            if (job.Category == category)
                result.Add(job);
        }

        return result;
    }

    public List<JobCategory> GetPresentedCategories()
    {
        List<JobCategory> result = new List<JobCategory>();
        foreach (Job job in Jobs)
        {
            if (!result.Contains(job.Category))
            {
                result.Add(job.Category);
            }
        }

        return result;
    }

    public void Init()
    {
        gameManager = GameManager.instance;
        gameDataManager = GameDataManager.instance;
        statusEffectsController = StatusEffectsController.instance;
        houseManager = HouseManager.instance;
        uiManager = UIManager.instance;
    }

    public void AddJob(Job job)
    {
        Jobs.Add(job);
        OnLaborExchangeStateChanged();
    }

    public void JobPanelClick(Job job)
    {

    }
}
