using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "SO/Items/BankServices/Loan", fileName = "Loan")]
public class Loan : BankService
{
    private static float rate = 0.20f;
    private bool canBePurchased = true;
    public float initialValue = 1000f;
    public string amountTitle = "Размер кредита";
    public float maxValue = 1000000f;
    public string buttonText = "Взять";
    public string amountValueText;
    public string amountInputFieldPlaceholderText;
    public string durationLabelText;
    public List<Dropdown.OptionData> durationDropdownOptions;
    private List<StatusEffect> statusEffects = new List<StatusEffect>();
    private int totalPeriod;

    private Loan()
    {
    }

    public class LoanBuilder : MonoBehaviour
    {
        Loan loan = new Loan();

        public LoanBuilder SetRate()
        {
            loan.Rate = rate;
            return this;
        }
        public LoanBuilder SetValue(float value)
        {
            loan.Value = value;
            return this;
        }
        public LoanBuilder SetTotalPeriod(int years)
        {
            loan.totalPeriod = years;
            return this;
        }
        public LoanBuilder AddStatusEffect(StatusEffect statusEffect)
        {
            loan.StatusEffects.Add(statusEffect);
            return this;
        }
        public float GetMonthlyPaymentValue()
        {
            float rateMonthlyPayment = loan.Rate / 12 * loan.Value;
            float debtMonthlyPayment = loan.Value / (loan.totalPeriod * 12);
            return -(rateMonthlyPayment + debtMonthlyPayment);
        }
        public Loan Build()
        {
            AddStatusEffect(new StatusEffect("Выплата кредита: " + loan.Value, 
                GetMonthlyPaymentValue(), 
                StatusEffectType.Money, 
                StatusEffectFrequency.Monthly));
            AddStatusEffect(new StatusEffect("Взято в кредит: " + loan.Value, 
                loan.Value, 
                StatusEffectType.Money, 
                StatusEffectFrequency.OneShot));
            SetValue(loan.Value);
            SetTotalPeriod(loan.totalPeriod);
            SetRate();
            return loan;
        }
    }
}
