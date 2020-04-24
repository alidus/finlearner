using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobExchangeViewFactory<T> : DefaultShowcaseViewFactory<T> where T : Job
{
    public JobExchangeViewFactory(Showcase<T> showcase, Object rootViewPrefab, Object itemGroupListViewPrefab, Object itemGroupViewPrefab, Object itemListViewPrefab, Object itemViewPrefab) : base(showcase, rootViewPrefab, itemGroupListViewPrefab, itemGroupViewPrefab, itemListViewPrefab, itemViewPrefab)
    {
    }

    public override View CreateItemListView(Transform parentTransform)
    {
        DefaultItemListView itemListView = GameObject.Instantiate(itemListViewPrefab as GameObject, parentTransform).GetComponent<DefaultItemListView>();
        itemListView.Init();
        RefreshItemListView(itemListView, itemListView.ScrollViewContentTransform);
        showcase.OnSelectedItemGroupChanged += delegate { RefreshItemListView(itemListView, itemListView.ScrollViewContentTransform); };
        return itemListView;
    }

    void RefreshItemListView(DefaultItemListView itemListView, Transform scrollViewContentTransform)
    {
        itemListView.DestroyItemViews();
        foreach (Job item in showcase.SelectedItemGroup.Items)
        {
            CreateItemView(item, itemListView.ScrollViewContentTransform);
        }
        itemListView.UpdateView();
    }

    public View CreateItemView(Job job, Transform parentTransform)
    {
        JobExchangeItemView jobExchangeItemView = GameObject.Instantiate(itemViewPrefab as GameObject, parentTransform).GetComponent<JobExchangeItemView>();

        jobExchangeItemView.Init(job);
        jobExchangeItemView.UpdateView();
        return jobExchangeItemView;
    }
}
