using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

[ExecuteInEditMode]
public class StoreItemPresenter : MonoBehaviour, IItemPresenter<ObjectItem>
{

    public Button ButtonComponent { get; set; }
    public Text TitleTextComponent { get; set; }
    public Text PriceTagTextComponent { get; set; }
    public Image IconImageComponent { get; set; }
    public Image OwnageIndicatorImageComponent { get; set; }

    public Image EqiupHighlgihtImageComponent { get; set; }
    public  ButtonClickedEvent OnClick { get => ButtonComponent.onClick; set => ButtonComponent.onClick = value; }

    public ObjectItem Item { get; set; }

    private void OnEnable()
    {
        Transform iconTransform = this.transform.Find("Icon");
        if (iconTransform)
        {
            Transform priceTagTransform = iconTransform.transform.Find("Price");
            if (priceTagTransform)
            {
                PriceTagTextComponent = priceTagTransform.GetComponentInChildren<Text>();
            }
            IconImageComponent = iconTransform.GetComponent<Image>();
            Transform OwnageIndicatorTransform = iconTransform.transform.Find("OwnIndicator");
            if (OwnageIndicatorTransform)
            {
                OwnageIndicatorImageComponent = OwnageIndicatorTransform.GetComponent<Image>();
            }
        }
        TitleTextComponent = this.transform.Find("TitleText").GetComponent<Text>();
        Transform equipHightlightTransform = this.transform.Find("EquipHighlightPanel");
        if (equipHightlightTransform)
        {
            EqiupHighlgihtImageComponent = equipHightlightTransform.GetComponent<Image>();
        }
        
    }

    public void UpdatePresenter()
    {
        if (Item != null)
        {
            if (PriceTagTextComponent)
                PriceTagTextComponent.text = "$" + Item.Price.ToString();
            if (IconImageComponent)
                IconImageComponent.sprite = Item.Sprite != null ? Item.Sprite : GameDataManager.instance.placeHolderSprite;
            if (OwnageIndicatorImageComponent)
                OwnageIndicatorImageComponent.enabled = Item.IsPurchased;
            if (TitleTextComponent)
                TitleTextComponent.text = Item.Title;
            if (EqiupHighlgihtImageComponent)
                EqiupHighlgihtImageComponent.enabled = Item.IsEquipped;
        }
    }
}
