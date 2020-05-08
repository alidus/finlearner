using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobExchangeViewFactory : DefaultShowcaseViewFactory<Job, JobExchange>
{
    public JobExchangeViewFactory(Showcase<Job, JobExchange> showcase, Object rootViewPrefab, Object itemGroupListViewPrefab, Object itemGroupViewPrefab, Object itemListViewPrefab, Object itemViewPrefab) : base(showcase, rootViewPrefab, itemGroupListViewPrefab, itemGroupViewPrefab, itemListViewPrefab, itemViewPrefab)
    {
    }

    public override DefaultItemListView CreateItemListView(Transform parentTransform)
    {
        DefaultItemListView itemListView = GameObject.Instantiate(itemListViewPrefab as GameObject, parentTransform).GetComponent<DefaultItemListView>();
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
