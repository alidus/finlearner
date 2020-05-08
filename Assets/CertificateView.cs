using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CertificateView : View
{
    public string Title { get; set; }
    public EducationEntityType EducationEntityType { get; set; }
    public EducationDirection EducationDirection { get; set; }

    private Certificate certificate;
    Text titleText;
    Text eduEntityTypeText;
    Text eduDirectonBg;

    Image titleBg;
    Image eduEntityTypeBg;
    Image eduDirectionBg;
    Image frameImage;


    private void OnEnable()
    {
        titleText = transform.Find("Background").Find("TitlePanel").Find("TitleText").GetComponent<Text>();
        eduEntityTypeText = transform.Find("Background").Find("EduEntityTypePanel").Find("EduEntityTypeText").GetComponent<Text>();
        eduDirectonBg = transform.Find("Background").Find("EduEntityDirectionPanel").Find("EduEntityTypeText").GetComponent<Text>();

        titleBg = transform.Find("Background").Find("TitlePanel").GetComponent<Image>();
        eduEntityTypeBg = transform.Find("Background").Find("EduEntityTypePanel").GetComponent<Image>();
        eduDirectionBg = transform.Find("Background").Find("EduEntityDirectionPanel").GetComponent<Image>();
        frameImage = GetComponent<Image>();

    }

    public void Init(Certificate certificate)
    {
        this.certificate = certificate;
        Title = certificate.Title;
        EducationEntityType = certificate.EducationEntityType;
        EducationDirection = certificate.EducationDirection;
        UpdateView();
    }

    public override void UpdateView()
    {
        UpdateTitle();
        UpdateEduEntityType();
        UpdateEduDirectionType();
        UpdateColorScheme();
    }

    private void UpdateColorScheme()
    {
        switch (EducationEntityType)
        {
            case EducationEntityType.Course:
                eduEntityTypeBg.color = GameDataManager.instance.CourseEducationEnityTypeColor;
                break;
            case EducationEntityType.Degree:
                eduEntityTypeBg.color = GameDataManager.instance.DegreeEducationEnityTypeColor;
                break;
            default:
                break;
        }

        switch (EducationDirection)
        {
            case EducationDirection.Technical:
                eduDirectionBg.color = GameDataManager.instance.TechnicalEducationDirectionTypeColor;
                break;
            case EducationDirection.Law:
                eduDirectionBg.color = GameDataManager.instance.LawEducationDirectionTypeColor;
                break;
            default:
                break;
        }
    }

    private void UpdateEduDirectionType()
    {
        eduDirectonBg.text = EducationDirection.ToString();
    }

    private void UpdateEduEntityType()
    {
        eduEntityTypeText.text = EducationEntityType.ToString();
    }

    private void UpdateTitle()
    {
        titleText.text = Title;
    }
}
