using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreViewFactory<T> : DefaultShowcaseViewFactory<T> where T : StoreItem
{
    public StoreViewFactory(Showcase<T> showcase, Object rootViewPrefab, Object itemGroupListViewPrefab, Object itemGroupViewPrefab, Object itemListViewPrefab, Object itemViewPrefab) 
        : base(showcase, rootViewPrefab, itemGroupListViewPrefab, itemGroupViewPrefab, itemListViewPrefab, itemViewPrefab)
    {
    }

    public override View CreateItemListView(Transform parentTransform)
    {
        DefaultItemListView itemListView = GameObject.Instantiate(itemListViewPrefab as GameObject, parentTransform).GetComponent<DefaultItemListView>();
        itemListView.Init();
        foreach (StoreItem item in showcase.SelectedItemGroup.Items)
        {
            CreateItemView(item, itemListView.ScrollViewContentTransform);
        }
        itemListView.UpdateView();
        return itemListView;
    }

    public View CreateItemView(StoreItem item, Transform parentTransform)
    {
        StoreItemView storeItemView = GameObject.Instantiate(itemViewPrefab as GameObject, parentTransform).GetComponent<StoreItemView>();
        
        storeItemView.Title = item.Title;
        storeItemView.Price = item.Price;
        storeItemView.Sprite = item.Sprite;
        storeItemView.IsPurchased = item.IsPurchased;
        storeItemView.IsEquipped = item.IsEquipped;

        var buttonComponent = storeItemView.GetComponent<Button>();
        var animator = storeItemView.GetComponent<Animator>();
        if (buttonComponent)
        {
            buttonComponent.onClick.AddListener(item.OnClick);
            // Play animation

            item.OnBuy += delegate {
                if (animator)
                {
                    animator.SetTrigger("Buy");
                }
                 };
            item.OnEquip += delegate
            {
                if (animator)
                {
                    animator.SetTrigger("Equip");
                }
            };
            item.OnUnEquip += delegate
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
