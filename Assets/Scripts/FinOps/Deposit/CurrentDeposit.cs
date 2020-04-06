using System.Collections.Generic;
using UnityEngine;

namespace FinOps.Deposit
{
    /* This deposit is for an indefinite amount of time and 5% annual interest rate.
     It can be deposited and withdrawn at any time, interest is paid monthly.*/
    public class CurrentDeposit : AbstractDeposit
    {
        private const float Rate = 0.05f;
        
        public CurrentDeposit()
        {
        }

        public class CurrentDepositBuilder : MonoBehaviour
        {
            CurrentDeposit currentDeposit = new CurrentDeposit();

            private CurrentDepositBuilder SetCurrentDepositValue(float value)
            {
                currentDeposit.DepositValue = value;
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
                    currentDeposit.DepositValue * Rate / 12, 
                    StatusEffectType.Money, 
                    StatusEffectFrequency.Monthly));
                SetCurrentDepositValue(currentDeposit.DepositValue);
                return currentDeposit;
            }
        }
    }
}