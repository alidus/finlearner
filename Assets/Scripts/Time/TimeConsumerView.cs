﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeConsumerView : View
{
    Text titleText;
    Image backgroundImage;
    ITimeConsumer timeConsumer;
    Animator animator;

    public Color sleepColor = new Color();
    public Color jobColor = new Color();
    public Color educationColor = new Color();


    public string Title { get; set; }
    public Color BackgroundColor { get; set; }


    private void OnEnable()
    {
        titleText = transform.Find("TitlePanel").Find("TitleText").GetComponent<Text>();
        backgroundImage = transform.Find("TitlePanel").GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    public void Init(ITimeConsumer timeConsumer)
    {
        this.timeConsumer = timeConsumer;
        switch (timeConsumer.TimeConsumerCategory)
        {
            case TimeConsumerCategory.Sleep:
                Title = "Сон";
                BackgroundColor = sleepColor;
                break;
            case TimeConsumerCategory.Job:
                Title = "Работа";
                BackgroundColor = jobColor;
                break;
            case TimeConsumerCategory.Education:
                Title = "Образование";
                BackgroundColor = educationColor;
                break;
            default:
                break;
        }

        UpdateView();
    }

    public override void UpdateView()
    {
        UpdateTitle();
        UpdateBGColor();
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        animator.SetBool("IsShown", true);
    }

    public void BeginDestroy()
    {
        animator.SetBool("IsShown", false);
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }

    private void UpdateBGColor()
    {
        backgroundImage.color = BackgroundColor;
    }

    private void UpdateTitle()
    {
        titleText.text = Title;
    }
}