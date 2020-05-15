using UnityEngine;

[CreateAssetMenu(menuName = "SO/Items/BankServices/CurrentDeposit", fileName = "CurrentDeposit")]
/* This deposit is for an indefinite amount of time and 5% annual interest rate.
 It can be deposited and withdrawn at any time, interest is paid monthly.*/
public class CurrentDeposit : Deposit, IPurchasable, IHaveStatusEffect
{
    protected override void GenerateStatusEffects()
    {
        StatusEffects.Clear();
        StatusEffects.Add(new StatusEffect("Первоначальный взнос по вкладу", -Amount, StatusEffectType.Money, StatusEffectFrequency.OneShot, StatusEffectFlags.Deposit));
        StatusEffects.Add(new StatusEffect("Ежемесячные выплаты по вкладу", (Rate * Amount) / 12, StatusEffectType.Money, StatusEffectFrequency.Monthly, StatusEffectFlags.Deposit));
    }
}