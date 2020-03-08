using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : MonoBehaviour
{
    private float yearRate;
    private float loanBalance;
    private float daysRemained;
    private List<Modifier> modifiers;

    private Credit(float value, float periodInDays, float yearRate, List<Modifier> modifiers)
    {
        this.yearRate = yearRate;
        this.loanBalance = value;
        this.daysRemained = periodInDays;
        this.modifiers = modifiers;
    }

    public Credit GetNewCredit(float value, int periodInDays, float yearRate, List<Modifier> modifiers)
    {
        Credit result = new Credit(value, periodInDays, yearRate, modifiers);

        return result;
    }


}
