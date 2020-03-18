using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StoreItemListPresenter : MonoBehaviour, IItemListPresenter<ObjectItem>
{
    public Store Store { get; set; }
    public ItemGroup<ObjectItem> ItemGroup { get; set; }

    public Transform ScrollViewContentTransform { get; set; }

    private void OnEnable()
    {
        Store = GetComponentInParent<Store>();
        var k = this.transform.Find("ScrollView");
        var z = this.transform.Find("ScrollView")?.Find("Viewport");
        var n = this.transform.Find("ScrollView")?.Find("Viewport")?.Find("Content");
        ScrollViewContentTransform = this.transform.Find("ScrollView")?.Find("Viewport")?.Find("Content") ?? null;

        UpdatePresenter();
    }
    public void UpdatePresenter() {
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
                    StoreFactory.CreateItemPresenter(item, ScrollViewContentTransform).UpdatePresenter();
                }
            } else
            {
                Debug.Log("Selected group missing in store");
            }

            
        }
    }
}
