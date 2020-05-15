using UnityEngine;

[CreateAssetMenu(menuName = "SO/Items/BankServices/TimeDeposit", fileName = "TimeDeposit")]
/* This deposit is for a 366 days period and 10% annual interest rate.
 It can be withdraw after the period is over else the interest is burned. */
public class TimeDeposit : BankService, IPurchasable, IHaveStatusEffect
{
    private const float rate = 0.1f;

    private int daysRemained;

    public int DaysRemained
    {
        get => daysRemained;
        set => daysRemained = value;
    }

    private const int TotalPeriod = 366;

    private TimeDeposit()
    {
    }

    public class TimeDepositBuilder : MonoBehaviour
    {
        TimeDeposit timeDeposit = new TimeDeposit();

        public TimeDepositBuilder SetTimeDepositValue(float value)
        {
            timeDeposit.Amount = value;
            return this;
        }
        private TimeDepositBuilder AddStatusEffect(StatusEffect statusEffect)
        {
            timeDeposit.StatusEffects.Add(statusEffect);
            return this;
        }
        private float GetYearlyPayment()
        {
            return timeDeposit.Amount * rate;
        }
        public TimeDeposit Build()
        {
            AddStatusEffect(new StatusEffect("Срочный вклад: " + timeDeposit.Amount, 
                GetYearlyPayment(), 
                StatusEffectType.Money, 
                StatusEffectFrequency.Yearly));
            SetTimeDepositValue(timeDeposit.Amount);
            return timeDeposit;
        }
    }
}