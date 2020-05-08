using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class DefaultItemView : View, IViewTitle
{
    public Text TitleTextComponent { get; set; }

    public string Title { get; set; }
    private void OnEnable()
    {
        TitleTextComponent = this.transform.Find("TitleText").GetComponent<Text>();
    }

    public virtual void Init(Item item)
    {
        Title = item.Title;
    }

    public override void UpdateView()
    {
        UpdateTitle();
    }

    public void UpdateTitle()
    {
        if (TitleTextComponent)
            TitleTextComponent.text = Title;
    }
}
