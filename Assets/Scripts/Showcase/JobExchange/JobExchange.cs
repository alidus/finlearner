using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JobExchange : Showcase<Job>
{
    private UIManager uiManager;
    private GameManager gameManager;
    private GameDataManager gameDataManager;
    private StatusEffectsManager statusEffectsController;
    private EnvironmentManager houseManager;

    JobExchangeViewFactory<Job> factory;

    // Events, Delegates
    public delegate void LaborExchangeStateChangedAction();
    public event LaborExchangeStateChangedAction OnLaborExchangeStateChanged;


    private void OnEnable()
    {
        ItemDatabase.Clear();
        // Load all jobs from assets
        foreach (Object job in Resources.LoadAll("ScriptableObjects/Jobs"))
        {
            ItemDatabase.Add(ScriptableObject.Instantiate(job) as Job);
        }

        // Setup groups
        if (ItemDatabase.Count > 0)
        {
            ItemGroups = GetItemGroups();
            if (ItemGroups.Count > 0)
                SelectedItemGroup = ItemGroups[0];
        }

        if (factory == null)
        {
            factory = new JobExchangeViewFactory<Job>(
                this,
                Resources.Load("Prefabs/JobExchange/Views/JobExchangeView"),
                Resources.Load("Prefabs/JobExchange/Views/JobExchangeItemGroupListView"),
                Resources.Load("Prefabs/JobExchange/Views/JobExchangeItemGroupView"),
                Resources.Load("Prefabs/JobExchange/Views/JobExchangeItemListView"),
                Resources.Load("Prefabs/JobExchange/Views/JobExchangeItemView"));
        }

        if (transform.childCount != 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        RootView = factory.CreateRootView(this.transform);
    }

    public override void UpdateShowcase()
    {
        if (RootView != null)
        {
            RootView.UpdateView();
        }
    }

    protected override List<ItemGroup<Job>> GetItemGroups()
    {
        var result = new List<ItemGroup<Job>>();
        foreach (Job job in ItemDatabase)
        {
            var itemGroup = result.FirstOrDefault(item => item.Title == job.Category.ToString());
            if (itemGroup == null)
            {
                itemGroup = new ItemGroup<Job>(job.Category.ToString());
                result.Add(itemGroup);
            }
            itemGroup.Add(job);
        }

        return result;
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
}
