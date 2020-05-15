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
    protected RectTransform textContentRT;
    protected RectTransform textViewportRT;

    public Action OnStateChanged;
    protected float runningTextSpeed = 50;
    protected float runningTextGlobalCooldown = 2;
    protected float runningTextCurrentCooldown = 2;

    protected bool isRunningText;
    protected float targetTextContentXPosition;

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
        textViewportRT = transform.Find("TextViewport").GetComponent<RectTransform>();
        textContentRT = textViewportRT.Find("TextContent").GetComponent<RectTransform>();
        textComponent = textContentRT.Find("Text").GetComponent<Text>();
        animator = GetComponent<Animator>();
        StartCoroutine(SetupIsRunningText());
    }

    protected IEnumerator SetupIsRunningText()
    {
        yield return null;
        isRunningText = textContentRT.rect.width > textViewportRT.rect.width;
    }

    void UpdateTextContentPosition()
    {
        if (runningTextCurrentCooldown > 0)
        {
            runningTextCurrentCooldown -= Time.deltaTime;
            if (runningTextCurrentCooldown <= 0)
            {
                // Cooldown will pass next frame
                targetTextContentXPosition = 0;
                textContentRT.anchoredPosition = new Vector3(targetTextContentXPosition, textContentRT.anchoredPosition.y);
            }
        } else
        {
            if (textContentRT.anchoredPosition.x + textContentRT.rect.width < textViewportRT.rect.width)
            { 
                // Running text reached destination, start cooldown
                runningTextCurrentCooldown = runningTextGlobalCooldown;
            }
            targetTextContentXPosition = textContentRT.anchoredPosition.x - runningTextSpeed * Time.deltaTime;

            textContentRT.anchoredPosition = new Vector3(targetTextContentXPosition, textContentRT.anchoredPosition.y);
        }
    }

    public virtual void SubscribeToConditionChanges()
    {

        switch (GameCondition)
        {
            case ValueCondition _:
                {
                    var valueCondition = (ValueCondition)GameCondition;
                    switch (valueCondition.TargetVariable)
                    {
                        case TargetVariable.Money:
                            gameDataManager.OnMoneyValueChanged -= GameDataValueChangedHandler;
                            gameDataManager.OnMoneyValueChanged += GameDataValueChangedHandler;
                            break;
                        case TargetVariable.Mood:
                            gameDataManager.OnMoodValueChanged -= GameDataValueChangedHandler;
                            gameDataManager.OnMoodValueChanged += GameDataValueChangedHandler;
                            break;
                        default:
                            break;
                    }
                }
                break;
            case OwnCondition _:
                {
                    GameCondition.OnStateChanged -= UpdateMilestone;
                    GameCondition.OnStateChanged += UpdateMilestone;
                }
                break;
            default:
                break;
        }

        GameCondition.OnStateChanged -= delegate { OnStateChanged(); };
        GameCondition.OnStateChanged += delegate { OnStateChanged(); };
    }

    private void OnDestroy()
    {
        if (GameCondition != null)
        {
            GameCondition.OnStateChanged -= UpdateMilestone;
            GameCondition.OnStateChanged -= delegate { OnStateChanged(); };
        }
    }

    public void GameDataValueChangedHandler(float value)
    {
        UpdateMilestone();
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

    private void Update()
    {
        if (isRunningText)
        {
            UpdateTextContentPosition();
        }
    }

    public void UpdateText(string text)
    {
        textComponent.text = text;
    }
}
