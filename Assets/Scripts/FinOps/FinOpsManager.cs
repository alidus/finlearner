//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FinOpsManager : MonoBehaviour
//{
//    public static FinOpsManager instance;

//    // Managers, Controllers
//    GameDataManager gameDataManager;
//    StatusEffectsManager statusEffectsManager;

//    private List<Loan> activeLoans = new List<Loan>();
//    public List<Loan> ActiveLoans { get => activeLoans; set => activeLoans = value; }

//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//        }
//        else if (instance != this)
//        {
//            Destroy(gameObject);
//        }
//        DontDestroyOnLoad(gameObject);
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        Init();
//    }

//    private void Init()
//    {
//        gameDataManager = GameDataManager.instance;
//        statusEffectsManager = StatusEffectsManager.instance;
//    }

//    public void GetLoan()
//    {
//        Loan.LoanBuilder builder = new Loan.LoanBuilder();
//        Loan loan = builder.SetRate()
//            .SetAmount(5000f)
//            .SetTotalPeriodInMonths(100)
//            .Build();
//        loan.StatusEffects.Add(
//            new StatusEffect("Выплата кредита", 
//                builder.GetMonthlyPaymentValue(), 
//                StatusEffectType.Money, 
//                StatusEffectFrequency.Monthly));
//        ActivateLoan(loan);
//    }

//    private void ActivateLoan(Loan loan)
//    {
//        gameDataManager.Money += loan.Value;
//        activeLoans.Add(loan);
//        statusEffectsManager.ApplyStatusEffects(loan.StatusEffects);
//    }
//}
