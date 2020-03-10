using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit
{
    private float yearRate;
    private float loanBalance;
    private float loanTotalAmount;
    public float LoanTotalAmount
    {
        get { return loanTotalAmount; }
        set { loanTotalAmount = value; }
    }
    private float daysRemained;
    private float billingPeriodInDays;
    private List<StatusEffect> statusEffects;
    public System.Collections.Generic.List<StatusEffect> StatusEffects
    {
        get { return statusEffects; }
        set { statusEffects = value; }
    }
    private Credit(float loanTotalAmount, float billingPeriodInDays, float yearRate)
    {
        this.yearRate = yearRate;
        this.loanBalance = loanTotalAmount;
        this.LoanTotalAmount = loanTotalAmount;
        this.billingPeriodInDays = billingPeriodInDays;
    }

    public static Credit Create(float value, int billingPeriodInDays, float yearRate)
    {
        Credit result = new Credit(value, billingPeriodInDays, yearRate);

        return result;
    }

    public float GetMonthlyPaymentAmount()
    {
        float rateDailyPayment = yearRate / 366 * LoanTotalAmount;
        float debtDailyPayment = LoanTotalAmount / 366;
        return billingPeriodInDays * (rateDailyPayment + debtDailyPayment);
    }
}
