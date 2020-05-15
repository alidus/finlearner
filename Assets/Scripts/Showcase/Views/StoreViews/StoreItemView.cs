using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class StoreItemView : DefaultItemView, IViewTitle, IViewImage, IViewPrice, IViewPurchaseState, IViewEquipState
{
    StoreItem storeItem;
    Button buttonComponent;
    public Text PriceTagTextComponent { get; set; }
    public Image IconImageComponent { get; set; }
    public Image OwnageIndicatorImageComponent { get; set; }
    public Image EqiupHighlgihtImageComponent { get; set; }

    Animator animator;

    public float Price { get; set; }
    public Sprite Sprite { get; set; }

    public bool IsPurchased { get; set; }
    public bool IsEquipped { get; set; }
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

        buttonComponent = GetComponent<Button>();
        animator = GetComponent<Animator>();
    }

    public override void Init(Item target)
    {
        storeItem = target as StoreItem;

        Title = storeItem.Title;
        Price = storeItem.Price;
        Sprite = storeItem.Sprite;
        IsPurchased = storeItem.IsPurchased;
        IsEquipped = storeItem.IsEquipped;

        buttonComponent.onClick.AddListener(delegate
        {
            if (storeItem.CanBeEquipped)
            {
                if (!storeItem.IsEquipped)
                {
                    storeItem.Equip();
                }
                else
                {
                    storeItem.Uneqip();
                }
            }
            else if (storeItem.CanBePurchased)
            {
                if (!storeItem.IsPurchased)
                {
                    storeItem.Purchase();
                }
            }
        });

        storeItem.OnPurchaseStateChanged -= PurchaseStateChangedHandler;
        storeItem.OnPurchaseStateChanged += PurchaseStateChangedHandler;

        storeItem.OnEquipStateChanged -= EquipStateChangedHandler;
        storeItem.OnEquipStateChanged += EquipStateChangedHandler;
    }

    private void OnDestroy()
    {
        if (storeItem)
        {
            storeItem.OnPurchaseStateChanged -= PurchaseStateChangedHandler;
            storeItem.OnEquipStateChanged -= EquipStateChangedHandler;
        }
    }

    void PurchaseStateChangedHandler()
    {
        IsPurchased = storeItem.IsPurchased;
        UpdateView();
    }

    void EquipStateChangedHandler()
    {
        IsEquipped = storeItem.IsEquipped;
        UpdateView();
    }

    public override void UpdateView()
    {
        UpdateTitle();
        UpdatePrice();
        UpdateImage();
        UpdatePurchasedState();
        UpdateAnimator();
        UpdateEquippedState();
    }

    public void UpdateAnimator()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("IsPurchased", IsPurchased);
        animator.SetBool("IsEquipped", IsEquipped);
    }

    public void UpdatePrice()
    {
        if (PriceTagTextComponent)
            PriceTagTextComponent.text = "$" + Price.ToString();
    }

    public void UpdateImage()
    {
        if (IconImageComponent)
            IconImageComponent.sprite = Sprite != null ? Sprite : GameDataManager.instance.placeHolderSprite;
    }

    public void UpdatePurchasedState()
    {
        if (OwnageIndicatorImageComponent)
            OwnageIndicatorImageComponent.enabled = IsPurchased;
    }

    public void UpdateEquippedState()
    {
        if (EqiupHighlgihtImageComponent)
            EqiupHighlgihtImageComponent.enabled =IsEquipped;
    }

}
