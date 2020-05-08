using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DefaultItemListView : View
{
    private Transform scrollViewContentTransform;

    public Transform ScrollViewContentTransform { 
        get => scrollViewContentTransform;
        set => scrollViewContentTransform = value; }
    [SerializeField]

    private void OnEnable()
    {
        ScrollViewContentTransform = transform.Find("ScrollView")?.Find("Viewport")?.Find("Content");
    }


    public virtual void DestroyItemViews()
    {
        var targetTransform = ScrollViewContentTransform;
        for (int i = targetTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(targetTransform.GetChild(i).gameObject);
        }
    }

    public override void UpdateView()
    {

    }
}
