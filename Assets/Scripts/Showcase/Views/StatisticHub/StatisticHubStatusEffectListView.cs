using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StatisticHubStatusEffectListView : DefaultItemListView
{
    public UnityEngine.Object statusEffectViewPrefab;
    Dictionary<StatusEffect, StatusEffectView> statusEffectViewsDict = new Dictionary<StatusEffect, StatusEffectView>();

    private void OnEnable()
    {


    }

    void StatusEffectsCollectionChangedHandler(System.Object sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateView();
    }

    private void Start()
    {
        // TODO: this is temp. Implement dynamic prefab injection from factory
        statusEffectViewPrefab = Resources.Load("Prefabs/StatisticsHub/Views/StatusEffectView");

        StatusEffectsManager.instance.StatusEffects.CollectionChanged -= StatusEffectsCollectionChangedHandler;
        StatusEffectsManager.instance.StatusEffects.CollectionChanged += StatusEffectsCollectionChangedHandler;

        UpdateView();
    }

    private void OnDestroy()
    {
        StatusEffectsManager.instance.StatusEffects.CollectionChanged -= StatusEffectsCollectionChangedHandler;
    }

    protected StatusEffectView CreateStatusEffectView(StatusEffect statusEffect)
    {
        Transform scrollViewTransform = statusEffect.Value > 0 ? 
            transform.Find("PositiveStatusEffectScrollView") :
            transform.Find("NegativeStatusEffectScrollView");
        var parentTransform = scrollViewTransform.Find("Viewport").Find("Content").transform;
        StatusEffectView statusEffectView = GameObject.Instantiate<GameObject>(statusEffectViewPrefab as GameObject, parentTransform).GetComponent<StatusEffectView>();
        statusEffectView.Init(statusEffect);
        return statusEffectView;
    }

    protected void RemoveStatusEffect(StatusEffect statusEffect)
    {
        if (statusEffectViewsDict.ContainsKey(statusEffect))
        {
            // Destroy view
            Destroy(statusEffectViewsDict[statusEffect].gameObject);
            statusEffectViewsDict.Remove(statusEffect);
        }

    }

    public override void DestroyItemViews()
    {
        foreach (StatusEffectView statusEffectView in GetComponentsInChildren<StatusEffectView>())
        {
            Destroy(statusEffectView.gameObject);
        }
    }


    public override void UpdateView()
    {
        UpdateStatusEffects();
    }

    private void UpdateStatusEffects()
    {
        // Add SEs views for each SE presented in manager but missing view in statistics hub
        foreach (StatusEffect statusEffect in StatusEffectsManager.instance.StatusEffects)
        {
            if (!statusEffectViewsDict.ContainsKey(statusEffect))
            {
                statusEffectViewsDict[statusEffect] = CreateStatusEffectView(statusEffect);
            }
        }
        // Remove SEs that are presented in statistics list but are missing in status effect manager
        foreach (StatusEffect statusEffect in statusEffectViewsDict.Keys.ToList().Except(StatusEffectsManager.instance.StatusEffects))
        {
            Destroy(statusEffectViewsDict[statusEffect].gameObject);
            statusEffectViewsDict.Remove(statusEffect);
        }
    }
}
