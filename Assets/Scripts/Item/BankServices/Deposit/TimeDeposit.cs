using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Items/BankServices/TimeDeposit", fileName = "TimeDeposit")]
/* This deposit is for a 366 days period and 10% annual interest rate.
 It can be withdraw after the period is over else the interest is burned. */
public class TimeDeposit : Deposit, IPurchasable, IHaveStatusEffect
{
    protected override void GenerateStatusEffects()
    {
        StatusEffects.Clear();
        StatusEffects.Add(new StatusEffect("Взнос по вкладу", -Amount, StatusEffectType.Money, StatusEffectFrequency.OneShot, StatusEffectFlags.Deposit));
    }
}