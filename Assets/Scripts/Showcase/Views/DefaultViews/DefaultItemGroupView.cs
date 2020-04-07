using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class DefaultItemGroupView : View, IViewTitle
{
    public Image ImageComponent { get; private set; }
    public Text TextComponent { get; private set; }

    public Button ButtonComponent { get; set; }
    public ButtonClickedEvent OnClick { get => ButtonComponent.onClick; set => ButtonComponent.onClick = value; }
    public string Title { get; set; }

    private void OnEnable()
    {
        ImageComponent = this.GetComponent<Image>();
        TextComponent = this.GetComponentInChildren<Text>();
        ButtonComponent = this.GetComponent<Button>();
    }

    public void Init(string title)
    {
        Title = title;
    }

    public override void UpdateView()
    {
        UpdateTitle();
    }

    public void UpdateTitle()
    {
        TextComponent.text = Title;
    }

}
