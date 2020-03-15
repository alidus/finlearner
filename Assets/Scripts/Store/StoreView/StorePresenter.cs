using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePresenter : MonoBehaviour, IItemsPresenter<ItemObject>
{
    public IItemGroupButtonsPresenter<ItemObject> StoreItemCategoryButtonsPresenter { get; set; }
    
    public IItemListPresenter<ItemObject> StoreItemsListPresenter {get; set;}

    public GameObject StoreGameObject { get; set; }

    public ItemDatabase<ItemObject> ItemDatabase { get; set; }

    public void OnEnable()
    {
        ItemDatabase = this.GetComponentInParent<Store>().ItemDatabase;
        StoreItemCategoryButtonsPresenter = StoreFactory.CreateStoreItemCategoryButtonsPresenter(this.transform);
        StoreItemsListPresenter = StoreFactory.CreateStoreItemsListPresenter(this.transform);
    }

    public void Update()
    {
        StoreItemCategoryButtonsPresenter.Update();
        StoreItemsListPresenter.Update();
    } 
}
