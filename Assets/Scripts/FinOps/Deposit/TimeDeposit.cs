using UnityEngine;

/* This deposit is for a 366 days period and 10% annual interest rate.
 It can be withdraw after the period is over else the interest is burned. */
namespace FinOps.Deposit
{
    public class TimeDeposit : AbstractDeposit
    {
        private const float Rate = 0.1f;

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
                timeDeposit.DepositValue = value;
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
                    timeDeposit.DepositValue, 
                    StatusEffectType.Money, 
                    StatusEffectFrequency.Monthly));
                SetTimeDepositValue(timeDeposit.DepositValue);
                return timeDeposit;
            }
        }
    }
}