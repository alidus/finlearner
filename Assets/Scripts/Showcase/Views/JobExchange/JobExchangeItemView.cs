using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class JobExchangeItemView : DefaultItemView, IViewTitle, IViewImage, IViewEquipState
{

    public Button ButtonComponent { get; set; }
    public Image IconImageComponent { get; set; }
    public Image EqiupHighlgihtImageComponent { get; set; }

    public string Description { get; set; }

    Text DescriptionText;

    public Sprite Sprite { get; set; }

    public bool IsEquipped { get; set; }
    private void OnEnable()
    {
        Transform iconTransform = this.transform.Find("Icon");
        if (iconTransform)
        {
            IconImageComponent = iconTransform.GetComponent<Image>();
        }
        var InfoPanel = transform.Find("Info").transform;
        DescriptionText = InfoPanel.Find("Desc").GetComponent<Text>();
        TitleTextComponent = InfoPanel.Find("Title").GetComponent<Text>();
        Transform equipHightlightTransform = this.transform.Find("EquipHighlightPanel");
        if (equipHightlightTransform)
        {
            EqiupHighlgihtImageComponent = equipHightlightTransform.GetComponent<Image>();
        }
    }

    public override void UpdateView()
    {
        UpdateTitle();
        UpdateDescription();
        UpdateImage();
        UpdateEquippedState();
    }

    public void UpdateImage()
    {
        if (IconImageComponent)
            IconImageComponent.sprite = Sprite != null ? Sprite : GameDataManager.instance.placeHolderSprite;
    }

    public void UpdateEquippedState()
    {
        if (EqiupHighlgihtImageComponent)
            EqiupHighlgihtImageComponent.enabled = IsEquipped;
    }

    public void UpdateDescription()
    {
        DescriptionText.text = Description;
    }

}
