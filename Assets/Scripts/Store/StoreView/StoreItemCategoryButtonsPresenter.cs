using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItemCategoryButtonsPresenter : MonoBehaviour, IItemGroupButtonsPresenter<ItemObject>
{
    public List<ItemGroup<ItemObject>> ItemGroups { get; set; }

    public void OnEnable()
    {
        ItemGroups = this.GetComponentInParent<Store>().ItemGroups;
    }

    public void Update() {
        DeleteAllChildrenOfCategorryButtonsPresenter();

        foreach (ItemGroup<ItemObject> itemGroup in ItemGroups)
        {
            var categoryButton = StoreFactory.CreateStoreItemCategoryButtonPresenter(this.transform);
            categoryButton.Title = itemGroup.Title;
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
