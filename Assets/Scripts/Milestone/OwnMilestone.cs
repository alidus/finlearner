using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OwnMilestone : Milestone
{
    Image imageComponent;
    OwnCondition ownCondition;

    public OwnCondition OwnCondition { get => ownCondition; set { ownCondition = value; GameCondition = value; }  }

    private void OnEnable()
    {
        textViewportRT = transform.Find("TextViewport").GetComponent<RectTransform>();
        textContentRT = textViewportRT.Find("TextContent").GetComponent<RectTransform>();
        textComponent = textContentRT.Find("Text").GetComponent<Text>();
        imageComponent = transform.Find("Image").GetComponent<Image>();
        animator = GetComponent<Animator>();
        isRunningText = textContentRT.rect.width > textViewportRT.rect.width;
        StartCoroutine(SetupIsRunningText());
    }

    public void UpdateImage(Sprite sprite)
    {
        imageComponent.sprite = sprite;
    }

    public override void UpdateMilestone()
    {
        if (OwnCondition != null && OwnCondition.TargetItem != null)
        {
            switch (OwnCondition.TargetItem)
            {
                case FurnitureItem _:
                    {
                        var furnitureItem = (FurnitureItem)OwnCondition.TargetItem;
                        // TODO: use iDrawable interface if Item 
                        UpdateImage(furnitureItem.Sprite);
                        if (OwnCondition.State)
                        {
                            UpdateText("Вы купили " + furnitureItem.Title);
                        }
                        else
                        {
                            UpdateText("Вы должны купить " + furnitureItem.Title);
                        }
                        SetState(OwnCondition.State);
                    }
                    break;
                case Job _:
                    {
                        var job = (Job)OwnCondition.TargetItem;
                        UpdateImage(job.Sprite);
                        if (OwnCondition.State)
                        {
                            UpdateText("Вы устроились на работу: " + job.Title);
                        }
                        else
                        {
                            UpdateText("Вы должны устроиться на работу: " + job.Title);
                        }
                        SetState(OwnCondition.State);
                    }
                    break;
                case Certificate _:
                    {
                        var certificate = (Certificate)OwnCondition.TargetItem;
                        if (OwnCondition.State)
                        {
                            UpdateText("Вы получили сертификат: " + certificate.Title);
                        }
                        else
                        {
                            UpdateText("Вы должны получить сертификат: " + certificate.Title);
                        }
                        SetState(OwnCondition.State);
                    }
                    break;
                default:
                    {
                        var item = OwnCondition.TargetItem;
                        if (OwnCondition.State)
                        {
                            UpdateText("Вы получили: " + item.Title);
                        }
                        else
                        {
                            UpdateText("Вы должны получить: " + item.Title);
                        }
                        SetState(OwnCondition.State);
                    }
                    break;
            }

        }
    }

    public override void SubscribeToConditionChanges()
    {
        if (OwnCondition != null)
        {
            OwnCondition.OnStateChanged += UpdateMilestone;
        }
    }
}
