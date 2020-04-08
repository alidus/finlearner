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
        foreach (Job item in showcase.SelectedItemGroup.Items)
        {
            CreateItemView(item, itemListView.ScrollViewContentTransform);
        }
        itemListView.UpdateView();
        return itemListView;
    }

    public View CreateItemView(Job job, Transform parentTransform)
    {
        JobExchangeItemView storeItemView = GameObject.Instantiate(itemViewPrefab as GameObject, parentTransform).GetComponent<JobExchangeItemView>();

        storeItemView.Title = job.Title;
        storeItemView.Sprite = job.Sprite;
        storeItemView.IsEquipped = job.IsEquipped;
        storeItemView.Description = job.Description;

        var buttonComponent = storeItemView.GetComponent<Button>();
        var animator = storeItemView.GetComponent<Animator>();
        if (buttonComponent)
        {
            buttonComponent.onClick.AddListener(job.OnClick);
            // Play animation

            job.OnEquip += delegate
            {
                if (animator)
                {
                    animator.SetTrigger("Equip");
                }
            };
            job.OnUnEquip += delegate
            {
                if (animator)
                {
                    animator.SetTrigger("UnEquip");
                }
            };
        }
        storeItemView.UpdateView();
        return storeItemView;
    }
}
