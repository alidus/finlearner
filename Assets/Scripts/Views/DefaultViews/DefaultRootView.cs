using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Default root view representing item groups list and item list
/// </summary>
public class DefaultRootView : View
{
    public View ItemGroupListView { get; set; }
    public View ItemListView { get; set;}
    /// <summary>
    /// Call this to clear views and instantiate new ones based on passed prefabs
    /// </summary>
    /// <param name="itemGroupListViewPrefab"></param>
    /// <param name="itemListViewPrefab"></param>
    public void Init()
    {

    }
    /// <summary>
    /// Update all views recursively
    /// </summary>
    public override void UpdateView()
    {
        if (ItemGroupListView != null && ItemListView != null)
        {
            ItemGroupListView.UpdateView();
            ItemListView.UpdateView();
        }
    }
}
