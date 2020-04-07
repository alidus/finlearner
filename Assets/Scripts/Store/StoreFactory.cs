using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ViewFactory
{
    public static Store storeComponent;

    //public static Store CreateStore(Transform containerTransform)
    //{
    //    storeComponent = containerTransform.gameObject.AddComponent<Store>();
    //    storeComponent.ItemGroups = storeComponent.GetObjectItemCategories();
    //    storeComponent.SelectedItemGroup = storeComponent.ItemGroups[0];
    //    return storeComponent;
    //}

    public static StoreView CreateBaseView(UnityEngine.Object presenter, Transform parentTransform)
    {
        StoreView storePresenterComponent = GameObject.Instantiate(presenter as GameObject, parentTransform).GetComponent<StoreView>(); ;
        return storePresenterComponent;
    }

    public static StoreItemView CreateItemPresenter(UnityEngine.Object presenter, ObjectItem item, Transform parentTransform)
    {
        StoreItemView storeItemPresenterComponent = GameObject.Instantiate(presenter as GameObject, parentTransform).GetComponent<StoreItemView>(); ;
        
        storeItemPresenterComponent.Title = item.Title;
        storeItemPresenterComponent.Price = item.Price;
        storeItemPresenterComponent.Sprite = item.Sprite;
        storeItemPresenterComponent.IsPurchased = item.IsPurchased;
        storeItemPresenterComponent.IsEquipped = item.IsEquipped;

        var buttonComponent = storeItemPresenterComponent.GetComponent<Button>();
        var animator = storeItemPresenterComponent.GetComponent<Animator>();
        if (buttonComponent)
        {
            buttonComponent.onClick.AddListener(item.OnClick);
            // Play animation

            item.OnBuy += delegate {
                if (animator)
                {
                    animator.SetTrigger("Buy");
                }
                GameDataManager.instance.Money -= item.Price;
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

        return storeItemPresenterComponent;
    }
    
    public static StoreItemGroupListView CreateGroupListPresenter(UnityEngine.Object presenter, Transform parentTransform)
    {
        StoreItemGroupListView storeItemCategoryButtonsPresenterComponent = GameObject.Instantiate(presenter as GameObject, parentTransform).GetComponent<StoreItemGroupListView>(); ;
        return storeItemCategoryButtonsPresenterComponent;
    }

    public static StoreItemListView CreateItemListPresenter(UnityEngine.Object presenter, Transform parentTransform)
    {
        StoreItemListView storeItemsListPresenterComponent = GameObject.Instantiate(presenter as GameObject, parentTransform).GetComponent<StoreItemListView>();
        return storeItemsListPresenterComponent;
    }

    public static StoreItemGroupView CreateItemGroupPresenter(UnityEngine.Object presenter, ItemGroup<ObjectItem> itemGroup, Transform parentTransform)
    {
        StoreItemGroupView storeItemCategoryButtonPresenterComponent = GameObject.Instantiate(presenter as GameObject, parentTransform).GetComponent<StoreItemGroupView>();
        storeItemCategoryButtonPresenterComponent.ItemGroup = itemGroup;
        return storeItemCategoryButtonPresenterComponent;
    }
}
