using System.Collections.Generic;
using UnityEngine;

public class LaborExchangeManager : MonoBehaviour
{
    public static LaborExchangeManager instance;
    private UIManager uiManager;
    private GameManager gameManager;
    private GameDataManager gameDataManager;
    private StatusEffectsController statusEffectsController;
    private EnvironmentManager houseManager;

    // Events, Delegates
    public delegate void LaborExchangeStateChangedAction();
    public event LaborExchangeStateChangedAction OnLaborExchangeStateChanged;

    private List<Job> globalJobsPool = new List<Job>();
    private List<Job> activeJobs = new List<Job>();
    public System.Collections.Generic.List<Job> ActiveJobs
    {
        get { return activeJobs; }
        private set { activeJobs = value; }
    }
    public System.Collections.Generic.List<Job> GlobalJobsPool
    {
        get { return globalJobsPool; }
       private set { globalJobsPool = value; }
    }

    private JobCategory selectedCategory;
    public JobCategory SelectedCategory
    {
        get { return selectedCategory; }
        set { if (selectedCategory != value) { selectedCategory = value; OnLaborExchangeStateChanged(); };  }
    }
    private void Awake()
    {
        if (GameDataManager.instance.DEBUG)
            Debug.Log("LaborExchangeManager awake");
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

    public void Init()
    {
        gameManager = GameManager.instance;
        gameDataManager = GameDataManager.instance;
        statusEffectsController = StatusEffectsController.instance;
        houseManager = EnvironmentManager.instance;
        uiManager = UIManager.instance;

        GlobalJobsPool = (Resources.Load("ScriptableObjects/Jobs/JobPool") as JobList).Jobs;
    }

    public List<Job> GetAllJobsOfCategory(JobCategory category)
    {
        List<Job> result = new List<Job>();
        foreach (Job job in GlobalJobsPool)
        {
            if (job.Category == category)
                result.Add(job);
        }

        return result;
    }

    public List<JobCategory> GetPresentedCategories()
    {
        List<JobCategory> result = new List<JobCategory>();
        foreach (Job job in GlobalJobsPool)
        {
            if (!result.Contains(job.Category))
            {
                result.Add(job.Category);
            }
        }

        return result;
    }

    private void GetJob(Job job)
    {
        if (!ActiveJobs.Contains(job))
        {
            ActiveJobs.Add(job);
            statusEffectsController.AddStatusEffects(job.StatusEffects);
            OnLaborExchangeStateChanged();
        }
    }

    private void QuitJob(Job job)
    {
        ActiveJobs.Remove(job);
        statusEffectsController.RemoveStatusEffects(job.StatusEffects);
        OnLaborExchangeStateChanged();
    }

    public void AddJobToGlobalPool(Job job)
    {
        GlobalJobsPool.Add(job);
        OnLaborExchangeStateChanged();
    }

    public bool IsJobActive(Job job)
    {
        return ActiveJobs.Contains(job);
    }

    public void JobPanelClick(Job job)
    {
        if (IsJobActive(job))
        {
            QuitJob(job);
        } else
        {
            GetJob(job);
        }
    }
}
