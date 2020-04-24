using UnityEngine;

[CreateAssetMenu(menuName = "SO/Items/BankServices/Loan", fileName = "Loan")]
public class Loan : BankService
{
    private const float RATE = 0.20f;
    private float debtValue;
    public float DebtValue { get { return debtValue; } set { debtValue = value; } }
    private int daysRemained;
    public int DaysRemained { get { return daysRemained; } set { daysRemained = value; } }
    [SerializeField] private int totalPeriod;
    public int TotalPeriod { get => totalPeriod; set => totalPeriod = value; }

    private Loan()
    {
    }

    public class LoanBuilder : MonoBehaviour
    {
        Loan loan = new Loan();

        public LoanBuilder SetRate()
        {
            loan.Rate = RATE;
            return this;
        }
        public LoanBuilder SetValue(float value)
        {
            loan.Value = value;
            return this;
        }
        public LoanBuilder SetTotalPeriod(int years)
        {
            loan.TotalPeriod = years;
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
