using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum TargetVariable { Money, Mood }

public enum ValueConditionOperator { Equals, LessThan, GreaterThan, GreaterThanOrEqualsTo, LessThanOrEqualsTo }

public class ValueCondition : GameCondition
{
    [SerializeField]
    private TargetVariable targetVariable;
    [SerializeField]
    private ValueConditionOperator valueConditionOperator;

    public TargetVariable TargetVariable
    {
        get { return targetVariable; }
        set { targetVariable = value; }
    }

    public ValueConditionOperator ValueConditionOperator { get => valueConditionOperator; set => valueConditionOperator = value; }
    public float TargetFloatValue { get => targetFloatValue; set => targetFloatValue = value; }
    public int TargetIntValue { get => targetIntValue; set => targetIntValue = value; }

    private int targetIntValue;
    private float targetFloatValue;

    public ValueCondition(GameDataManager gameDataManager) : base(gameDataManager)
    {
       
    }

    /// <summary>
    /// Call this when managers are initialized so it will create callback controlling GameCondition's state
    /// </summary>
    public void Activate()
    {
        switch (targetVariable)
        {
            case TargetVariable.Money:
                gameDataManager.OnMoneyValueChanged += delegate
                {
                    State = CompareValues(gameDataManager.Money, targetFloatValue, valueConditionOperator);
                };
                break;
            case TargetVariable.Mood:
                gameDataManager.OnMoodValueChanged += delegate
                {
                    State = CompareValues(gameDataManager.Mood, targetFloatValue, valueConditionOperator);
                };
                break;
            default:
                break;
        }
    }

    public bool CompareValues(float a, float b, ValueConditionOperator op)
    {
        switch (op)
        {
            case ValueConditionOperator.Equals:
                return a == b;
            case ValueConditionOperator.LessThan:
                return a < b;
            case ValueConditionOperator.GreaterThan:
                return a > b;
            case ValueConditionOperator.GreaterThanOrEqualsTo:
                return a >= b;
            case ValueConditionOperator.LessThanOrEqualsTo:
                return a <= b;
            default:
                return false;
        }
    }
}
