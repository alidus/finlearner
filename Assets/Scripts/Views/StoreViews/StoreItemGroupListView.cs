using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItemGroupListView : View
{
    public Store Store { get; set; }
    public List<ItemGroup<ObjectItem>> ItemGroups { get; set; }
    [SerializeField]
    private Object itemGroupObject;

    private void OnEnable()
    {
        Store = GetComponentInParent<Store>();
        if (itemGroupObject == null)
            itemGroupObject = Resources.Load("Prefabs/Store/Views/StoreItemGroupView");
    }

    public override void UpdateView() {
        if (Store)
        {
            ItemGroups = Store.ItemGroups;
            if (ItemGroups != null)
            {
                DeleteAllChildrenOfCategorryButtonsPresenter();

                foreach (ItemGroup<ObjectItem> itemGroup in ItemGroups)
                {
                    PresenterFactory.CreateItemGroupPresenter(itemGroupObject, itemGroup, this.transform).UpdateView();
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
