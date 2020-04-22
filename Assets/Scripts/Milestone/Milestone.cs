using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Milestone : MonoBehaviour
{
    protected GameDataManager gameDataManager;
    private Action updateDelegate;
    protected Text textComponent;
    protected Animator animator;
    bool state;
    private GameCondition gameCondition;
    

    public GameCondition GameCondition { get => gameCondition; set => gameCondition = value; }

    public virtual bool GetState()
    {
        return state;
    }

    public virtual void SetState(bool value)
    {
        state = value;
        animator.SetBool("IsComplete", state);
    }

    private void Awake()
    {
        gameDataManager = GameDataManager.instance;
    }

    private void OnEnable()
    {
        textComponent = transform.Find("Text").GetComponent<Text>();
        animator = GetComponent<Animator>();
    }

    public virtual void SubscribeToConditionChanges()
    {
        GameCondition.OnStateChanged -= UpdateMilestone;
        GameCondition.OnStateChanged += UpdateMilestone;
    }

    public virtual void UpdateMilestone()
    {
        if (GameCondition != null && GameCondition is ValueCondition)
        {
            ValueCondition valueCondition = GameCondition as ValueCondition;
            switch (valueCondition.TargetVariable)
            {
                case TargetVariable.Money:
                    UpdateText("Вы заработали " +
                    gameDataManager.Money.ToString() + "$ из " +
                    valueCondition.TargetFloatValue.ToString() + "$");
                    SetState(valueCondition.State);
                    break;
                case TargetVariable.Mood:
                    UpdateText("Ваш уровень счастья " +
                    gameDataManager.Mood.ToString() + " из " +
                    valueCondition.TargetFloatValue.ToString());
                    SetState(valueCondition.State);
                    break;
                default:
                    break;
            }
        }
    }

    public void UpdateText(string text)
    {
        textComponent.text = text;
    }
}
