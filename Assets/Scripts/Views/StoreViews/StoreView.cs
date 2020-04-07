using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreView : View
{
    public StoreItemGroupListView StoreItemCategoryButtonsPresenter { get; set; }
    public StoreItemListView StoreItemsListPresenter {get; set;}
    [SerializeField]
    private Object itemGroupsPresenterObject;
    [SerializeField]
    private Object itemListPresenterObject;

    private void OnEnable()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        if (itemGroupsPresenterObject == null)
            itemGroupsPresenterObject = Resources.Load("Prefabs/Store/Views/StoreItemGroupListView");
        if (itemListPresenterObject == null)
            itemListPresenterObject = Resources.Load("Prefabs/Store/Views/StoreItemListView");
        StoreItemCategoryButtonsPresenter = ViewFactory.CreateGroupListPresenter(itemGroupsPresenterObject, this.transform);
        StoreItemsListPresenter = ViewFactory.CreateItemListPresenter(itemListPresenterObject, this.transform);

        UpdateView();
    }

    public override void UpdateView()
    {
        if (StoreItemCategoryButtonsPresenter != null && StoreItemsListPresenter != null)
        {
            StoreItemCategoryButtonsPresenter.UpdateView();
            StoreItemsListPresenter.UpdateView();
        }
    }
}
