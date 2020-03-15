using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItemsListPresenter : MonoBehaviour, IItemListPresenter<ItemObject>
{
    public ItemDatabase<ItemObject> ItemDatabase { get; private set; }

    private void OnEnable()
    {
        ItemDatabase = this.GetComponentInParent<Store>().ItemDatabase;
    }

    public void Update() {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (ItemObject item in this.GetComponentInParent<Store>().SelectedItemGroup)
        {
            // TODO: optimize not to clear entire array of gameObjects on every update but based on changes made
            StoreFactory.CreateItemPresenter(item, this.transform);
        }
    }
}
