using UnityEngine;
using UnityEngine.UI;

namespace Showcase.Views.BankViews
{
    public class BankViewFactory : DefaultShowcaseViewFactory<BankService, Bank.Bank>
    {
        Object loanViewPrefab;
        Object currentDepositViewPrefab;
        Object timeDepositViewPrefab;

        public BankViewFactory(Showcase<BankService, Bank.Bank> showcase, 
            Object rootViewPrefab, 
            Object itemGroupListViewPrefab, 
            Object itemGroupViewPrefab, 
            Object itemListViewPrefab, 
            Object loanViewPrefab,
            Object currentDepositViewPrefab,
            Object timeDepositViewPrefab) : 
            base(showcase, 
                rootViewPrefab, 
                itemGroupListViewPrefab, 
                itemGroupViewPrefab, 
                itemListViewPrefab, 
                null)
        {
            this.loanViewPrefab = loanViewPrefab;
            this.currentDepositViewPrefab = currentDepositViewPrefab;
            this.timeDepositViewPrefab = timeDepositViewPrefab;
        }

        public override View CreateRootView(Transform parentTransform)
        {
            Console.Print("___Start building bank___");
            rootView = GameObject.Instantiate(rootViewPrefab as GameObject, parentTransform).GetComponent<BankView>();
            itemGroupListView = CreateItemGroupListView(rootView.transform);
            itemListView = CreateItemListView(rootView.transform);
            showcase.OnSelectedItemGroupChanged -= UpdateItemListView;
            showcase.OnSelectedItemGroupChanged += UpdateItemListView;
            rootView.UpdateView();
            return rootView;
        }

        public View CreateLoanItemView(Transform parentTransform, Loan loanItem)
        {
            LoanView loanView = GameObject.Instantiate(loanViewPrefab as GameObject, parentTransform).GetComponent<LoanView>();

            loanView.Init(loanItem);
            loanView.UpdateView();

            return loanView;
        }

        public View CreateCurrentDepositItemView(Transform parentTransform, CurrentDeposit currentDeposit)
        {
            return null;
        }

        public View CreateTimeDepositItemView(Transform parentTransform, TimeDeposit timeDeposit)
        {
            return null;
        }

        public override View CreateItemView(Item item, Transform parentTransform)
        {
            switch (item)
            {
                case Loan _:
                    return CreateLoanItemView(parentTransform, (Loan)item);
                case TimeDeposit _:
                    return CreateTimeDepositItemView(parentTransform, (TimeDeposit)item);
                default:
                    return null;
            }
        }

    }
}
