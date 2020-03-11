using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loan
{
    private float rate;
    private float debtValue;
    public float DebtValue
    {
        get { return debtValue; }
        set { debtValue = value; }
    }
    private float initialValue;
    public float InitialValue
    {
        get { return initialValue; }
        set { initialValue = value; }
    }
    private int daysRemained;
    public int DaysRemained
    {
        get { return daysRemained; }
        set { daysRemained = value; }
    }
    private int totalPeriod;
    private List<StatusEffect> statusEffects = new List<StatusEffect>();
    public System.Collections.Generic.List<StatusEffect> StatusEffects
    {
        get { return statusEffects; }
        set { statusEffects = value; }
    }

    private Loan()
    {

    }


    public float GetMonthlyPaymentValue()
    {
        float rateDailyPayment = rate / 366 * InitialValue;
        float debtDailyPayment = InitialValue / 366;
        return totalPeriod * (rateDailyPayment + debtDailyPayment);
    }

    public class LoanBuilder : MonoBehaviour
    {
        Loan credit = new Loan();

        public LoanBuilder SetRate(float rate)
        {
            credit.rate = rate;
            return this;
        }
        public LoanBuilder SetInitialValue(float value)
        {
            credit.InitialValue = value;
            return this;
        }
        public LoanBuilder SetPeriod(int days)
        {
            credit.totalPeriod = days;
            return this;
        }
        public LoanBuilder AddStatusEffect(StatusEffect statusEffect)
        {
            credit.StatusEffects.Add(statusEffect);
            return this;
        }
        public Loan Build()
        {
            return credit;
        }
    }
}
