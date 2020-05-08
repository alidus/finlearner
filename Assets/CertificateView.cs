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
                eduDirectionBg.color = new Color(1f, 0, 0, 0.4f);
                break;
            case EducationEntityType.Degree:
                eduDirectionBg.color = new Color(1f, 0, 0.7f, 0.4f);
                break;
            default:
                break;
        }

        switch (EducationDirection)
        {
            case EducationDirection.Technical:
                eduEntityTypeBg.color = new Color(0.12f, 0.13f, 0.68f, 0.7f);
                break;
            case EducationDirection.Law:
                eduEntityTypeBg.color = new Color(0.84f, 0.62f, 0, 0.7f);
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
