using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItemCategoryButtonsPresenter : MonoBehaviour, IItemGroupButtonsPresenter<ObjectItem>
{
    public Store Store { get; set; }
    public List<ItemGroup<ObjectItem>> ItemGroups { get; set; }

    private void OnEnable()
    {
        Store = GetComponentInParent<Store>();
    }

    public void UpdatePresenter() {
        if (Store)
        {
            ItemGroups = Store.ItemGroups;
            if (ItemGroups != null)
            {
                DeleteAllChildrenOfCategorryButtonsPresenter();

                foreach (ItemGroup<ObjectItem> itemGroup in ItemGroups)
                {
                    StoreFactory.CreateStoreItemCategoryButtonPresenter(itemGroup, this.transform).UpdatePresenter();
                }
            }
        }
    }

    private void DeleteAllChildrenOfCategorryButtonsPresenter()
    {
        foreach (Transform categoryButtonTransform in this.transform)
        {
            GameObject.Destroy(categoryButtonTransform.gameObject);
        }
    }
}
