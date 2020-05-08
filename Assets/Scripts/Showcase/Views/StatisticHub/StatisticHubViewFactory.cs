using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticHubViewFactory : DefaultShowcaseViewFactory<Item, StatisticsHub>
{
    Object statusEffectListViewPrefab;
    Object statusEffectViewPrefab;
    public StatisticHubViewFactory(Showcase<Item, StatisticsHub> showcase, Object rootViewPrefab, Object itemGroupListViewPrefab, Object itemGroupViewPrefab, Object statusEffectListViewPrefab, Object statusEffectViewPrefab) 
        : base(showcase, rootViewPrefab, itemGroupListViewPrefab, itemGroupViewPrefab, null, null)
    {
        this.statusEffectListViewPrefab = statusEffectListViewPrefab;
        this.statusEffectViewPrefab = statusEffectViewPrefab;
    }

    public override View CreateRootView(Transform parentTransform)
    {
        Console.Print("___Start building inventory___");

        rootView = GameObject.Instantiate(rootViewPrefab as GameObject, parentTransform).GetComponent<DefaultRootView>();
        itemGroupListView = CreateItemGroupListView(rootView.transform);
        itemListView = CreateItemListView(rootView.transform);
        UpdateItemListView();
        rootView.UpdateView();
        return rootView;
    }

    public override DefaultItemListView CreateItemListView(Transform parentTransform)
    {
        if (showcase.SelectedItemGroup.Title == "Status effects")
        {
            return CreateStatusEffectsList(parentTransform);
        }

        return null;
    }

    public DefaultItemListView CreateStatusEffectsList(Transform parentTransform)
    {
        StatisticHubStatusEffectListView StatusEffectsListView = GameObject.Instantiate(statusEffectListViewPrefab as GameObject, parentTransform).GetComponent<StatisticHubStatusEffectListView>();

        return StatusEffectsListView;
    }
}
