using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DefaultItemListView : View
{
    public Transform ScrollViewContentTransform { get; set; }
    [SerializeField]

    private void OnEnable()
    {
        ScrollViewContentTransform = this.transform.Find("ScrollView")?.Find("Viewport")?.Find("Content") ?? null;
    }

    public void Init()
    {
    }
    public override void UpdateView()
    {

    }
}
