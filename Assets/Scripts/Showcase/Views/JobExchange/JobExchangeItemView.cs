using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class JobExchangeItemView : DefaultItemView, IViewTitle, IViewImage, IViewEquipState
{
    Job job;
    public Image IconImageComponent { get; set; }
    public Image EqiupHighlgihtImageComponent { get; set; }

    Animator animator;

    Button applyQuitButton;
    Text applyQuitButtonText;

    public string Description { get; set; }

    Text DescriptionText;

    public Sprite Sprite { get; set; }

    public bool IsEquipped { get; set; }
    public bool IsEquippable { get; set; }

    public SalaryDisplayView SalaryDisplayView;

    private void OnEnable()
    {
        var dataPanel = transform.Find("Data");
        var iconPanel = dataPanel.Find("IconPanel");
        IconImageComponent = iconPanel.Find("Icon").GetComponent<Image>();
        var infoPanel = dataPanel.Find("Info");
        var textInfo = infoPanel.Find("TextInfo");
        TitleTextComponent = textInfo.Find("Title").GetComponent<Text>();
        DescriptionText = textInfo.Find("Desc").GetComponent<Text>();
        SalaryDisplayView = GetComponentInChildren<SalaryDisplayView>();

        Transform equipHightlightTransform = this.transform.Find("EquipHighlightPanel");
        if (equipHightlightTransform)
        {
            EqiupHighlgihtImageComponent = equipHightlightTransform.GetComponent<Image>();
        }
        var controlPanel = transform.Find("ControlPanel");
        animator = GetComponent<Animator>();

        applyQuitButton = controlPanel.Find("ApplyQuitButton").GetComponent<Button>();
        applyQuitButtonText = applyQuitButton.transform.Find("Text").GetComponent<Text>();
    }


    public void Init(Job job)
    {
        this.job = job;
        Title = job.Title;
        Sprite = job.Sprite;
        IsEquipped = job.IsEquipped;
        Description = job.Description;
        IsEquippable = job.CanBeEquipped;
        IsEquipped = job.IsEquipped;
        SalaryDisplayView.UpdateValues(job);

        job.OnEquipStateChanged -= EquipStateChangedHandler;
        job.OnEquipStateChanged += EquipStateChangedHandler;

        job.OnEquippableStateChanged -= EquippableStateChangedHandler;
        job.OnEquippableStateChanged += EquippableStateChangedHandler;

        applyQuitButton.onClick.RemoveAllListeners();
        applyQuitButton.onClick.AddListener(delegate
        {
            if (job.CanBeEquipped)
            {
                if (!job.IsEquipped)
                {
                    job.Equip();
                }
                else
                {
                    job.Uneqip();
                }
            }
        });
    }

    private void OnDestroy()
    {
        if (job)
        {
            job.OnEquipStateChanged -= EquipStateChangedHandler;
            job.OnEquipStateChanged -= EquipStateChangedHandler;
            job.OnEquippableStateChanged -= EquippableStateChangedHandler;
        }
    }

    void EquipStateChangedHandler()
    {
        IsEquipped = job.IsEquipped;
        UpdateView();
    }

    void EquippableStateChangedHandler()
    {
        IsEquippable = job.CanBeEquipped;
        UpdateView();
    }

    public override void UpdateView()
    {
        UpdateTitle();
        UpdateDescription();
        UpdateImage();
        UpdateEquippedState();
        UpdateAnimator();
        UpdateButtons();
        SalaryDisplayView.UpdateView();
    }

    public void UpdateButtons()
    {
        applyQuitButtonText.text = IsEquippable ? "Уволиться" : "Устроиться";
    }

    public void UpdateAnimator()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("IsAvailable", IsEquippable);
        animator.SetBool("IsActive", IsEquipped);
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
