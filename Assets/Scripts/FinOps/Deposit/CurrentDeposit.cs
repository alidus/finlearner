using System.Collections.Generic;
using UnityEngine;

namespace FinOps.Deposit
{
    /* This deposit is for an indefinite amount of time and 5% annual interest rate.
     It can be deposited and withdrawn at any time, interest is paid monthly.*/
    public class CurrentDeposit
    {
        private const float Rate = 0.05f;
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
        
        private List<StatusEffect> statusEffects = new List<StatusEffect>();

        public List<StatusEffect> StatusEffects
        {
            get => statusEffects;
            set => statusEffects = value;
        }

        public CurrentDeposit()
        {
        }

        public class CurrentDepositBuilder : MonoBehaviour
        {
            CurrentDeposit currentDeposit = new CurrentDeposit();

            public CurrentDepositBuilder SetCurrentDepositValue(float value)
            {
                currentDeposit.depositValue = value;
                return this;
            }
            private CurrentDepositBuilder AddStatusEffect(StatusEffect statusEffect)
            {
                currentDeposit.StatusEffects.Add(statusEffect);
                return this;
            }
            public CurrentDeposit Build()
            {
                AddStatusEffect(new StatusEffect("Current Deposit", 
                    currentDeposit.depositValue, 
                    StatusEffectType.Money, 
                    StatusEffectFrequency.Monthly));
                return currentDeposit;
            }
        }
    }
}