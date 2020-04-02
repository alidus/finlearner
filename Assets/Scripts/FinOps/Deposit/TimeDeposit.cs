using System.Collections.Generic;
using UnityEngine;

/* This deposit is for a 366 days period and 10% annual interest rate.
 It can be withdraw after the period is over else the interest is burned. */
public class TimeDeposit
{
    private const float Rate = 0.1f;
    private float depositValue;

    public float DepositValue
    {
        get => depositValue;
        set => depositValue = value;
    }

    private int daysRemained;

    public int DaysRemained
    {
        get => daysRemained;
        set => daysRemained = value;
    }

    private const int TotalPeriod = 366;
    private List<StatusEffect> statusEffects = new List<StatusEffect>();

    public List<StatusEffect> StatusEffects
    {
        get => statusEffects;
        set => statusEffects = value;
    }

    private TimeDeposit()
    {
    }

    public class TimeDepositBuilder : MonoBehaviour
    {
        TimeDeposit timeDeposit = new TimeDeposit();

        public TimeDepositBuilder SetTimeDepositValue(float value)
        {
            timeDeposit.depositValue = value;
            return this;
        }
        private TimeDepositBuilder AddStatusEffect(StatusEffect statusEffect)
        {
            timeDeposit.StatusEffects.Add(statusEffect);
            return this;
        }
        public TimeDeposit Build()
        {
            AddStatusEffect(new StatusEffect("Time Deposit", 
                timeDeposit.depositValue, 
                StatusEffectType.Money, 
                StatusEffectFrequency.Monthly));
            return timeDeposit;
        }
    }
}