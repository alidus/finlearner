using UnityEngine;
using UnityEngine.UI;

namespace Showcase.Views.BankViews
{
    public class BankViewFactory<T> : DefaultShowcaseViewFactory<T> where T : BankService
    {
        public BankViewFactory(Showcase<T> showcase, 
            Object rootViewPrefab, 
            Object itemGroupListViewPrefab, 
            Object itemGroupViewPrefab, 
            Object itemListViewPrefab, 
            Object itemViewPrefab) : 
            base(showcase, 
                rootViewPrefab, 
                itemGroupListViewPrefab, 
                itemGroupViewPrefab, 
                itemListViewPrefab, 
                itemViewPrefab)
        {
        }

        public override View CreateItemListView(Transform parentTransform)
        {
            DefaultItemListView itemListView = GameObject.Instantiate(
                itemListViewPrefab as GameObject, parentTransform).GetComponent<DefaultItemListView>();
            itemListView.Init();
            foreach (var item in showcase.SelectedItemGroup.Items)
            {
                CreateItemView(item, itemListView.ScrollViewContentTransform);
            }
            itemListView.UpdateView();
            return itemListView;
        }

        public View CreateItemView(BankService service, Transform parentTransform)
        {
            var bankServiceView = GameObject.Instantiate(
                itemViewPrefab as GameObject, parentTransform).GetComponent<BankServiceView>();
        
            bankServiceView.Title = service.Title;

            var buttonComponent = bankServiceView.GetComponent<Button>();
            var animator = bankServiceView.GetComponent<Animator>();
            if (buttonComponent)
            {
                buttonComponent.onClick.AddListener(service.OnClick);
                // Play animation

                service.OnBuy += delegate {
                    if (animator)
                    {
                        animator.SetTrigger("Buy");
                    }
                };
            }
            bankServiceView.UpdateView();
            return bankServiceView;
        }

        public View CreateServiceView(Transform parentTransform)
        {
            DefaultRootView loanView = GameObject
                .Instantiate(rootViewPrefab as GameObject, parentTransform)
                .GetComponent<DefaultRootView>();
            loanView.Init();
            loanView.ItemGroupListView = CreateItemGroupListView(loanView.transform);
            loanView.ItemListView = CreateItemListView(loanView.transform);
            loanView.UpdateView();
            return loanView;
        }
    }
}
