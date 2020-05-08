using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatisticsHub : Showcase<Item, StatisticsHub>
{
    StatisticHubViewFactory factory;
    Animator animator;
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        // Load store database array containing different StoreItem databases of various item types (like clothing, furniture, etc)
        ItemDatabase.Clear();
        ItemDatabase = LoadAssets();

        // Setup groups
        ItemGroups = FormItemGroups();
        if (ItemGroups.Count > 0)
            SelectedItemGroup = ItemGroups[0];

        if (factory == null)
        {
            factory = new StatisticHubViewFactory(
                this,
                Resources.Load("Prefabs/StatisticsHub/Views/StatisticHubView"),
                Resources.Load("Prefabs/StatisticsHub/Views/StatisticHubItemGroupListView"),
                Resources.Load("Prefabs/StatisticsHub/Views/StatisticHubItemGroupView"),
                Resources.Load("Prefabs/StatisticsHub/Views/StatisticHubStatusEffectsListView"),
                Resources.Load("Prefabs/StatisticsHub/Views/StatusEffectView"));
        }

        DestroyViews();

        RootView = factory.CreateRootView(this.transform);
        
    }

    protected override ItemDatabase<Item> LoadAssets()
    {
        ItemDatabase<Item> result = new ItemDatabase<Item>();
        Console.Print("Loading statistics hub database...");
        
        Console.Print("Statistics hub database loaded");
        return result;
    }


    public override void UpdateShowcase()
    {
        if (RootView != null)
        {
            RootView.UpdateView();
        }
    }

    protected List<ItemGroup<Item>> FormItemGroups()
    {
        List<ItemGroup<Item>> result = new List<ItemGroup<Item>>();
        result.Add(new ItemGroup<Item>("Status effects"));
        return result;
    }

    protected override void AddItemsToDatabase(Item item)
    {
        base.AddItemsToDatabase(item);
        if (item is StatusEffect)
        {

        }
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

