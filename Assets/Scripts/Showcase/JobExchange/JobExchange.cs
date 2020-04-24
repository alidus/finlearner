﻿using System.Collections.Generic;
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
    Animator animator;

    // Events, Delegates
    public delegate void LaborExchangeStateChangedAction();
    public event LaborExchangeStateChangedAction OnLaborExchangeStateChanged;

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

    protected override ItemDatabase<Job> LoadAssets()
    {
        ItemDatabase<Job> result = new ItemDatabase<Job>();
        foreach (Job job in Resources.LoadAll("ScriptableObjects/JobExchange/Jobs"))
        {
            var jobInstance = ScriptableObject.Instantiate(job) as Job;
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
                ActiveJobs.Add(job);
                RegisterJob(job);
            }
        } else
        {
            if (isInActiveList)
            {
                // Remove from active job list
                ActiveJobs.Remove(job);
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

    protected override List<ItemGroup<Job>> FormItemGroups()
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
        StatusEffectsManager.instance.ApplyStatusEffects(job.StatusEffects);
    }

    void UnregisterJob(Job job)
    {
        StatusEffectsManager.instance.RemoveStatusEffects(job.StatusEffects);
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
