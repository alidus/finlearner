using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public enum EducationEntityType { Course, Degree }


public class EducationHubItemView : DefaultItemView, IViewTitle
{

    EducationEntity educationEntity;
    public Image IconImageComponent { get; set; }
    public Image EqiupHighlgihtImageComponent { get; set; }

    Animator animator;
    Slider progressBarSlider;

    Button applyQuitButton;
    Text applyQuitButtonText;
    Image eduEntityTypeBackground;
    Text eduEntityTypeText;
    Text priceTagText;

    public string Description { get; set; }

    Text DescriptionText;

    public Sprite Sprite { get; set; }

    public bool IsPurchased { get; set; }
    public bool IsAvailable { get; set; }

    public bool IsComplete { get; set; }

    public float Price { get; set; }

    public float CurrentProgress { get; set; }

    public EducationEntityType EducationEntityType { get; set; }

    private void OnEnable()
    {
        var dataPanel = transform.Find("Data");
        var iconTransform = dataPanel.Find("Icon");
        IconImageComponent = iconTransform.GetComponent<Image>();
        var infoPanel = dataPanel.Find("Info");
        var textInfo = infoPanel.Find("TextInfo");
        TitleTextComponent = textInfo.Find("TitlePanel").Find("Title").GetComponent<Text>();
        DescriptionText = textInfo.Find("DescPanel").Find("Desc").GetComponent<Text>();
        var valuesInfo = infoPanel.Find("ValuesInfo");
        eduEntityTypeBackground = valuesInfo.GetComponent<Image>();
        eduEntityTypeText = valuesInfo.Find("EduEntityType").GetComponent<Text>();
        priceTagText = valuesInfo.Find("PriceTag").Find("Text").GetComponent<Text>();

        Transform equipHightlightTransform = this.transform.Find("EquipHighlightPanel");
        if (equipHightlightTransform)
        {
            EqiupHighlgihtImageComponent = equipHightlightTransform.GetComponent<Image>();
        }
        var controlPanel = transform.Find("ControlPanel");
        progressBarSlider = controlPanel.Find("Slider").GetComponent<Slider>();
        animator = GetComponent<Animator>();

        applyQuitButton = controlPanel.Find("ApplyQuitButton").GetComponent<Button>();
        applyQuitButtonText = applyQuitButton.transform.Find("Text").GetComponent<Text>();
    }


    public override void Init(Item educationEntityItem)
    {
        var educationEntity = educationEntityItem as EducationEntity;
        this.educationEntity = educationEntity;
        Title = educationEntity.Title;
        Sprite = educationEntity.Sprite;
        Description = educationEntity.Description;
        IsAvailable = educationEntity.CanBePurchased;
        IsPurchased = educationEntity.IsPurchased;
        IsComplete = educationEntity.IsComplete;
        Price = educationEntity.Price;
        CurrentProgress = educationEntity.CurrentProgress;
        if (educationEntity is EduCourse)
        {
            EducationEntityType = EducationEntityType.Course;
        } else if (educationEntity is EduDegree)
        {
            EducationEntityType = EducationEntityType.Degree;
        }
        
        applyQuitButton.onClick.RemoveAllListeners();
        applyQuitButton.onClick.AddListener(delegate
        {
            if (educationEntity.CanBePurchased)
            {
                if (!educationEntity.IsPurchased)
                {
                    var confirmationHint = HintsManager.instance.ShowHint("Вы хотите начать " + educationEntity.Title + "?",
                        String.Format("Обучение будет стоить {0}. После его прохождения вы получите следующие сертификаты:\n", educationEntity.Price) + educationEntity.Certificates.ToString(), HintType.Confirmation);
                    (confirmationHint.View as IHintAccept).OnAccept += educationEntity.Purchase;
                } else
                {
                    var confirmationHint = HintsManager.instance.ShowHint("Вы хотите прекратить обучение " + educationEntity.Title + "?",
                        "Вы потеряете деньги, вложенные в обучение, а также не получите следующие сертификаты:\n" + educationEntity.Certificates.ToString(), HintType.Confirmation);
                    (confirmationHint.View as IHintAccept).OnAccept += educationEntity.Sell;
                }
            }
        });

        educationEntity.OnPurchaseStateChanged -= PurchaseStateChangedHandler;
        educationEntity.OnPurchaseStateChanged += PurchaseStateChangedHandler;

        educationEntity.OnProgressChanged -= UpdateProgressBar;
        educationEntity.OnProgressChanged += UpdateProgressBar;

        educationEntity.OnComplete -= HandleCompleteStateChanged;
        educationEntity.OnComplete += HandleCompleteStateChanged;

        educationEntity.OnPurchasableStateChanged -= PurchasableStateChangedHandler;
        educationEntity.OnPurchasableStateChanged += PurchasableStateChangedHandler;
    }

    private void PurchasableStateChangedHandler()
    {
        IsAvailable = educationEntity.CanBePurchased;
        UpdateView();
    }

    private void HandleCompleteStateChanged()
    {
        IsComplete = educationEntity.IsComplete;
        educationEntity.OnProgressChanged -= UpdateProgressBar;
        UpdateView();
    }


    private void OnDestroy()
    {
        if (educationEntity)
        {
            educationEntity.OnPurchaseStateChanged -= PurchaseStateChangedHandler;
            educationEntity.OnProgressChanged -= UpdateProgressBar;
            educationEntity.OnComplete -= HandleCompleteStateChanged;
            educationEntity.OnPurchasableStateChanged -= PurchasableStateChangedHandler;
        }
    }

    void PurchaseStateChangedHandler()
    {
        IsPurchased = educationEntity.IsPurchased;
        UpdateView();
    }

    public override void UpdateView()
    {
        UpdateTitle();
        UpdateDescription();
        UpdateImage();
        UpdateEquippedState();
        UpdateEduEntityType();
        UpdateAnimator();
        UpdatePrice();
        UpdateButtons();
        UpdateProgressBar();
    }

    public void UpdateProgressBar()
    {
        progressBarSlider.normalizedValue = educationEntity.CurrentProgress;
    }

    public void UpdatePrice()
    {
        priceTagText.text = "$" + Price;
    }

    public void UpdateEduEntityType()
    {
        switch (EducationEntityType)
        {
            case EducationEntityType.Course:
                eduEntityTypeText.text = "Курс";
                eduEntityTypeBackground.color = GameDataManager.instance.CourseEducationEnityTypeColor;
                break;
            case EducationEntityType.Degree:
                eduEntityTypeText.text = "Степень";
                eduEntityTypeBackground.color = GameDataManager.instance.DegreeEducationEnityTypeColor;
                break;
            default:
                break;
        }
    }

    public void UpdateButtons()
    {
        if (!IsComplete)
        {
            applyQuitButtonText.text = educationEntity.IsPurchased ? "Прекратить обучение" : "Начать обучение";
        } else
        {
            applyQuitButtonText.text = "Обучение завершено";
        }
    }

    public void UpdateAnimator()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("IsAvailable", IsAvailable);
        animator.SetBool("IsInProgress", IsPurchased);
        animator.SetBool("IsComplete", IsComplete);
    }

    public void UpdateImage()
    {
        if (IconImageComponent)
            IconImageComponent.sprite = Sprite != null ? Sprite : GameDataManager.instance.placeHolderSprite;
    }

    public void UpdateEquippedState()
    {
        if (EqiupHighlgihtImageComponent)
            EqiupHighlgihtImageComponent.enabled = IsPurchased;
    }

    public void UpdateDescription()
    {
        DescriptionText.text = Description;
    }
}
