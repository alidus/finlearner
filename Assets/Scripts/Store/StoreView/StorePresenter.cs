using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StorePresenter : MonoBehaviour, IItemsPresenter<ObjectItem>
{

    public Store Store { get; set; }

    public StoreItemCategoryButtonsPresenter StoreItemCategoryButtonsPresenter { get; set; }
    
    public StoreItemListPresenter StoreItemsListPresenter {get; set;}

    private void OnEnable()
    {
        Store = GetComponentInParent<Store>();
        if (StoreItemCategoryButtonsPresenter == null)
            StoreItemCategoryButtonsPresenter = StoreFactory.CreateStoreItemCategoryButtonsPresenter(this.transform);

        if (StoreItemsListPresenter == null)
            StoreItemsListPresenter = StoreFactory.CreateStoreItemsListPresenter(this.transform);


        UpdatePresenter();
    }


    public void UpdatePresenter()
    {
        if (StoreItemCategoryButtonsPresenter != null && StoreItemsListPresenter != null)
        {
            StoreItemCategoryButtonsPresenter.UpdatePresenter();
            StoreItemsListPresenter.UpdatePresenter();
        }
    }
}
