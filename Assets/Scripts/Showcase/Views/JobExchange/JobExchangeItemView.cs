using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class JobExchangeItemView : DefaultItemView, IViewTitle, IViewImage, IViewEquipState
{
    Job job;
    public Button ButtonComponent { get; set; }
    public Image IconImageComponent { get; set; }
    public Image EqiupHighlgihtImageComponent { get; set; }

    Animator animator;

    Button applyQuitButton;
    Text applyQuitButtonText;

    public string Description { get; set; }

    Text DescriptionText;

    public Sprite Sprite { get; set; }

    public bool IsActive { get; set; }
    public bool IsAvailable { get; set; }

    public SalaryDisplayView SalaryDisplayView;

    private void OnEnable()
    {
        var dataPanel = transform.Find("Data");
        var iconTransform = dataPanel.Find("Icon");
        IconImageComponent = iconTransform.GetComponent<Image>();
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
        IsActive = job.IsEquipped;
        Description = job.Description;
        IsAvailable = job.CanBeEquipped;
        IsActive = job.IsEquipped;
        SalaryDisplayView.UpdateValues(job);

        job.OnEquipStateChanged += JobStateChangedHandler;
        applyQuitButton.onClick.AddListener(job.OnClick);
    }

    private void OnDestroy()
    {
        job.OnEquipStateChanged -= JobStateChangedHandler;
    }

    void JobStateChangedHandler()
    {
        IsActive = job.IsEquipped;
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
        applyQuitButtonText.text = job.IsEquipped ? "Уволиться" : "Устроиться";
    }

    public void UpdateAnimator()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("IsAvailable", IsAvailable);
        animator.SetBool("IsActive", IsActive);
    }

    public void UpdateImage()
    {
        if (IconImageComponent)
            IconImageComponent.sprite = Sprite != null ? Sprite : GameDataManager.instance.placeHolderSprite;
    }

    public void UpdateEquippedState()
    {
        if (EqiupHighlgihtImageComponent)
            EqiupHighlgihtImageComponent.enabled = IsActive;
    }

    public void UpdateDescription()
    {
        DescriptionText.text = Description;
    }

}
