using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Items/BankServices/TimeDeposit", fileName = "TimeDeposit")]
/* This deposit is for a 366 days period and 10% annual interest rate.
 It can be withdraw after the period is over else the interest is burned. */
public class TimeDeposit : BankService, IPurchasable, IHaveStatusEffect
{
    int daysLeftToRepay = -1;

    protected override void GenerateStatusEffects()
    {
        StatusEffects.Add(new StatusEffect("Вы оформили депозит", -Amount, StatusEffectType.Money, StatusEffectFrequency.OneShot, StatusEffectFlags.Deposit));
    }

    public float GetMonthlyPaymentValue()
    {
        float rateMonthlyPayment = Rate / TotalPeriodInMonths * Amount;
        float debtMonthlyPayment = Amount / TotalPeriodInMonths;
        return rateMonthlyPayment + debtMonthlyPayment;
    }


}