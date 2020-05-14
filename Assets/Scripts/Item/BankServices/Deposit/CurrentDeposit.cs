using UnityEngine;

[CreateAssetMenu(menuName = "SO/Items/BankServices/CurrentDeposit", fileName = "CurrentDeposit")]
/* This deposit is for an indefinite amount of time and 5% annual interest rate.
 It can be deposited and withdrawn at any time, interest is paid monthly.*/
public class CurrentDeposit : BankService
{
    private new const float Rate = 0.05f;
    public CurrentDeposit()
    {
    }

    public class CurrentDepositBuilder : MonoBehaviour
    {
        CurrentDeposit currentDeposit = new CurrentDeposit();

        private CurrentDepositBuilder SetCurrentDepositValue(float value)
        {
            currentDeposit.Value = value;
            return this;
        }
        private CurrentDepositBuilder AddStatusEffect(StatusEffect statusEffect)
        {
            currentDeposit.StatusEffects.Add(statusEffect);
            return this;
        }
        public float GetMonthlyPaymentValue()
        {
            float rateMonthlyPayment = Rate / 12 * currentDeposit.Value;
            return rateMonthlyPayment;
        }
        public CurrentDeposit Build()
        {
            AddStatusEffect(new StatusEffect("Бессрочный вклад: " + currentDeposit.Value, 
                GetMonthlyPaymentValue(), 
                StatusEffectType.Money, 
                StatusEffectFrequency.Monthly));
            SetCurrentDepositValue(currentDeposit.Value);
            return currentDeposit;
        }
    }
}