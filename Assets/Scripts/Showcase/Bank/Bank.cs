using System;
using System.Collections.Generic;
using Showcase.Views.BankViews;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Video;

namespace Showcase.Bank
{
    /// <summary>
    /// Showcase that displays BankObject items
    /// </summary>
    public class Bank : Showcase<BankService, Bank>
    {
        Animator animator;

        private BankViewFactory factory;
        private Asset loanAsset;

        private void OnEnable()
        {
            animator = GetComponent<Animator>();

            ItemDatabase.Clear();
            // Load all services from assets
            ItemDatabase = LoadAssets();

            // Setup groups
            if (ItemDatabase.Count > 0)
            {
                FormItemGroups();
                if (ItemGroups.Count > 0)
                    SelectedItemGroup = ItemGroups[0];
            }

            if (factory == null)
            {
                factory = new BankViewFactory(
                    this,
                    Resources.Load("Prefabs/Bank/Views/BankView"),
                    Resources.Load("Prefabs/Bank/Views/BankItemGroupListView"),
                    Resources.Load("Prefabs/Bank/Views/BankItemGroupView"),
                    Resources.Load("Prefabs/Bank/Views/BankItemListView"),
                    Resources.Load("Prefabs/Bank/Views/BankLoanView"),
                    null,
                    null);
            }

            if (transform.childCount != 0)
            {
                for (int i = transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
            }
            RootView = factory.CreateRootView(transform);
        }

        private void FormItemGroups()
        {
            foreach (BankService item in ItemDatabase)
            {
                var itemGroup = FindOrCreateItemGroup(item.GetType().ToString());
                itemGroup.Add(item);
            }
        }

        private void OnDisable()
        {
            foreach (Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public override void UpdateShowcase()
        {
            if (RootView != null)
            {
                RootView.UpdateView();
            }
        }


        protected override ItemDatabase<BankService> LoadAssets()
        {
            Console.Print("Loading bank services...");
            var result = new ItemDatabase<BankService>();
            foreach (var service in Resources.LoadAll("ScriptableObjects/Bank/Catalog"))
            {
                result.Add(Instantiate(service) as BankService);
            }

            return result; 
        }

        public void TakeLoan(Loan loan)
        {
            loan.Purchase();
            Debug.Log(loan.ToString() + ": loan taken!");
        }

        public override void Toggle()
        {
            if (animator)
            {
                animator.SetBool("IsOpened", !animator.GetBool("IsOpened"));
            }
        }

        public override void Show()
        {
            if (animator)
            {
                animator.SetBool("IsOpened", true);
            }
        }

        public override void Hide()
        {
            if (animator)
            {
                animator.SetBool("IsOpened", false);
            }
        }
    }
}

