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
            if (OwnCondition.TargetItem is FurnitureItem)
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
            } else if (OwnCondition.TargetItem is Job)
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
