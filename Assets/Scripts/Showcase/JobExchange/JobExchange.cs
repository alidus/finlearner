using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JobExchange : Showcase<Job, JobExchange>
{
    private UIManager uiManager;
    private GameManager gameManager;
    private GameDataManager gameDataManager;
    private StatusEffectsManager statusEffectsController;
    private EnvironmentManager houseManager;

    JobExchangeViewFactory factory;
    Animator animator;


    List<Job> activeJobs = new List<Job>();
    public List<Job> ActiveJobs { get => activeJobs; set => activeJobs = value; }

    private void OnEnable()
    {
        animator = GetComponent<Animator>();

        ItemDatabase.Clear();
        ItemDatabase = LoadAssets();

        // Setup groups
        if (ItemDatabase.Count > 0)
        {
            ItemGroups = FormItemGroups();
            if (ItemGroups.Count > 0)
                SelectedItemGroup = ItemGroups[0];
        }

        if (factory == null)
        {
            factory = new JobExchangeViewFactory(
                this,
                Resources.Load("Prefabs/JobExchange/Views/JobExchangeView"),
                Resources.Load("Prefabs/JobExchange/Views/JobExchangeItemGroupListView"),
                Resources.Load("Prefabs/JobExchange/Views/JobExchangeItemGroupView"),
                Resources.Load("Prefabs/JobExchange/Views/JobExchangeItemListView"),
                Resources.Load("Prefabs/JobExchange/Views/JobExchangeItemView"));
        }

        DestroyViews();

        RootView = factory.CreateRootView(this.transform);
    }

    private void Start()
    {
        gameDataManager = GameDataManager.instance;
    }

    protected override ItemDatabase<Job> LoadAssets()
    {
        ItemDatabase<Job> result = new ItemDatabase<Job>();
        foreach (Job job in Resources.LoadAll("ScriptableObjects/JobExchange/Jobs"))
        {
            var jobInstance = ScriptableObject.Instantiate(job) as Job;
            jobInstance.Init();
            jobInstance.OnEquipStateChanged += delegate { HandleJobActivationChange(jobInstance); };

            jobInstance.OnEquipStateChanged -= delegate { job.NotifyOnInstanceEquipStateChanged(jobInstance); };
            jobInstance.OnEquipStateChanged += delegate { job.NotifyOnInstanceEquipStateChanged(jobInstance); };

            jobInstance.OnEquippableStateChanged -= delegate { job.NotifyOnInstanceEquippableStateChanged(jobInstance); };
            jobInstance.OnEquippableStateChanged += delegate { job.NotifyOnInstanceEquippableStateChanged(jobInstance); };
            result.Add(jobInstance);
        }

        return result;
    }

    void HandleJobActivationChange(Job job)
    {
        bool isInActiveList = ActiveJobs.Contains(job);
            if (job.IsEquipped)
        {
            if (!isInActiveList)
            {
                // Add to active job list
                RegisterJob(job);
            }
        } else
        {
            if (isInActiveList)
            {
                // Remove from active job list
                UnregisterJob(job);
            }
        }
    }

    public override void UpdateShowcase()
    {
        if (RootView != null)
        {
            RootView.UpdateView();
        }
    }

    protected List<ItemGroup<Job>> FormItemGroups()
    {
        var result = new List<ItemGroup<Job>>();
        foreach (Job job in ItemDatabase)
        {
            var itemGroup = result.FirstOrDefault(group => group.Title == job.Category.Title);
            if (itemGroup == null)
            {
                itemGroup = new ItemGroup<Job>(job.Category.Title);
                result.Add(itemGroup);
            }
            itemGroup.Add(job);
        }

        return result;
    }

    void RegisterJob(Job job)
    {
        ActiveJobs.Add(job);
        StatusEffectsManager.instance.ApplyStatusEffects(job.StatusEffects);
        gameDataManager.AddTimeConsumers(job);
    }

    void UnregisterJob(Job job)
    {
        ActiveJobs.Remove(job);
        StatusEffectsManager.instance.RemoveStatusEffects(job.StatusEffects);
        gameDataManager.RemoveTimeConsumers(job);
    }

    private void OnDisable()
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public List<Job> GetAllJobsOfCategory(JobCategory category)
    {
        List<Job> result = new List<Job>();
        foreach (Job job in ItemDatabase)
        {
            if (job.Category == category)
                result.Add(job);
        }

        return result;
    }

    public List<JobCategory> GetPresentedCategories()
    {
        List<JobCategory> result = new List<JobCategory>();
        foreach (Job job in ItemDatabase)
        {
            if (!result.Contains(job.Category))
            {
                result.Add(job.Category);
            }
        }

        return result;
    }

    public override void Toggle()
    {
        if (animator)
        {
            animator.SetBool("IsOpened", !animator.GetBool("IsOpened"));
        }
    }

    public override void Show()
    {
        if (animator)
        {
            animator.SetBool("IsOpened", true);
        }
    }

    public override void Hide()
    {
        if (animator)
        {
            animator.SetBool("IsOpened", false);
        }
    }
}
