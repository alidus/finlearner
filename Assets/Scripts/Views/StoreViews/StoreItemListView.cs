using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StoreItemListView : View
{
    public Store Store { get; set; }
    public ItemGroup<ObjectItem> ItemGroup { get; set; }

    public Transform ScrollViewContentTransform { get; set; }
    [SerializeField]
    private Object itemPresenterObject;

    private void OnEnable()
    {
        Store = GetComponentInParent<Store>();
        var k = this.transform.Find("ScrollView");
        var z = this.transform.Find("ScrollView")?.Find("Viewport");
        var n = this.transform.Find("ScrollView")?.Find("Viewport")?.Find("Content");
        ScrollViewContentTransform = this.transform.Find("ScrollView")?.Find("Viewport")?.Find("Content") ?? null;
        if (itemPresenterObject == null)
        {
            itemPresenterObject = Resources.Load("Prefabs/Store/Views/StoreItemView");
        }
        UpdateView();
    }
    public override void UpdateView() {
        if (Store)
        {
            ItemGroup = Store.SelectedItemGroup;
            if (ItemGroup != null && ScrollViewContentTransform)
            {
                foreach (Transform child in ScrollViewContentTransform)
                {
                    Destroy(child.gameObject);
                }
                foreach (ObjectItem item in ItemGroup)
                {
                    // TODO: optimize not to clear entire array of gameObjects on every update but based on changes made
                    PresenterFactory.CreateItemPresenter(itemPresenterObject, item, ScrollViewContentTransform).UpdateView();
                }
            } else
            {
                Debug.Log("Selected group missing in store");
            }

            
        }
    }
}
