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
        textComponent = transform.Find("Text").GetComponent<Text>();
        imageComponent = transform.Find("Image").GetComponent<Image>();
        animator = GetComponent<Animator>();
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
