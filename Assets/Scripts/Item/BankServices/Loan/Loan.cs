using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "SO/Items/BankServices/Loan", fileName = "Loan")]
public class Loan : BankService, IPurchasable, IHaveStatusEffect
{
    [SerializeField]
    float mixAmount = 1000f;
    public float MixAmount
    {
        get { return mixAmount; }
        set { mixAmount = value; }
    }
    [SerializeField]
    float maxAmount = 10000f;
    public float MaxAmount
    {
        get { return maxAmount; }
        set { maxAmount = value; }
    }

    [SerializeField]
    float rate = 0.20f;
    public float Rate { get => rate; set => rate = value; }

    private List<StatusEffect> statusEffects = new List<StatusEffect>();
    [SerializeField]
    int totalPeriodInMonths = 12;
    public int TotalPeriodInMonths
    {
        get { return totalPeriodInMonths; }
        set { totalPeriodInMonths = value; }
    }
    int daysLeftToRepay = -1;

    void GenerateStatusEffects()
    {
        StatusEffects.Add(new StatusEffect("Деньги в кредит", Amount, StatusEffectType.Money, StatusEffectFrequency.OneShot, StatusEffectFlags.Loan));
        StatusEffects.Add(new StatusEffect("Выплата кредита", -GetMonthlyPaymentValue(), StatusEffectType.Money, StatusEffectFrequency.Monthly, StatusEffectFlags.Loan));
    }

    public float GetMonthlyPaymentValue()
    {
        float rateMonthlyPayment = Rate / TotalPeriodInMonths * Amount;
        float debtMonthlyPayment = Amount / TotalPeriodInMonths;
        return rateMonthlyPayment + debtMonthlyPayment;
    }

    public override void Purchase()
    {
        GenerateStatusEffects();
        base.Purchase();
    }


    public class LoanBuilder : MonoBehaviour
    {
        Loan loan = new Loan();

        public LoanBuilder SetRate(float rate)
        {
            loan.Rate = rate;
            return this;
        }
        public LoanBuilder SetAmount(float amount)
        {
            loan.Amount = amount;
            return this;
        }
        public LoanBuilder SetTotalPeriodInMonths(int months)
        {
            loan.TotalPeriodInMonths = months;
            return this;
        }
        public LoanBuilder AddStatusEffect(StatusEffect statusEffect)
        {
            loan.StatusEffects.Add(statusEffect);
            return this;
        }
        
        public Loan Build()
        {
            AddStatusEffect(new StatusEffect("Выплата кредита: " + loan.Amount, 
                loan.GetMonthlyPaymentValue(), 
                StatusEffectType.Money, 
                StatusEffectFrequency.Monthly));
            AddStatusEffect(new StatusEffect("Взято в кредит: " + loan.Amount, 
                loan.Amount, 
                StatusEffectType.Money, 
                StatusEffectFrequency.OneShot));
            return loan;
        }

    }
}
