using System.Collections.Generic;
using Showcase.Views.BankViews;
using UnityEngine;

namespace Showcase.Bank
{
    /// <summary>
    /// Showcase that displays BankObject items
    /// </summary>
    public class Bank : Showcase<BankService>
    {
        private BankViewFactory<BankService> factory;

        private void OnEnable()
        {
            ItemDatabase.Clear();
            // Load all services from assets
            foreach (var service in Resources.LoadAll("ScriptableObjects/Bank/Catalog"))
            {
                ItemDatabase.Add(Instantiate(service) as BankService);
            }

            // Setup groups
            if (ItemDatabase.Count > 0)
            {
                ItemGroups = GetItemGroups();
                if (ItemGroups.Count > 0)
                    SelectedItemGroup = ItemGroups[0];
            }

            if (factory == null)
            {
                factory = new BankViewFactory<BankService>(
                    this,
                    Resources.Load("Prefabs/Store/Views/BankView"),
                    Resources.Load("Prefabs/Store/Views/BankServiceGroupListView"),
                    Resources.Load("Prefabs/Store/Views/BankServiceGroupView"),
                    Resources.Load("Prefabs/Store/Views/BankServiceListView"),
                    Resources.Load("Prefabs/Store/Views/BankServiceView"));
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

        protected override List<ItemGroup<BankService>> GetItemGroups()
        {
            var result = new List<ItemGroup<BankService>>();
            foreach (BankService item in ItemDatabase)
            {
                switch (item)
                {
                    case Loan _:
                    {
                        var k = result[0].GetType().GetGenericTypeDefinition();
                        var groupOfType = result.Find(group => @group.Items[0] is Loan);
                        if (groupOfType != null) { groupOfType.Add(item); }
                        else
                        {
                            var newGroup = new ItemGroup<BankService> {Title = "Кредит"};
                            newGroup.Add(item);
                            result.Add(newGroup);
                        }
                        break;
                    }
                    case CurrentDeposit _:
                    {
                        var k = result[0].GetType().GetGenericTypeDefinition();
                        var groupOfType = result.Find(group => @group.Items[0] is CurrentDeposit);
                        if (groupOfType != null) { groupOfType.Add(item); }
                        else
                        {
                            var newGroup = new ItemGroup<BankService> {Title = "Бессрочный вклад"};
                            newGroup.Add(item);
                            result.Add(newGroup);
                        }
                        break;
                    }
                    case TimeDeposit _:
                    {
                        var k = result[0].GetType().GetGenericTypeDefinition();
                        var groupOfType = result.Find(group => @group.Items[0] is TimeDeposit);
                        if (groupOfType != null) { groupOfType.Add(item); }
                        else
                        {
                            var newGroup = new ItemGroup<BankService> {Title = "Срочный вклад"};
                            newGroup.Add(item);
                            result.Add(newGroup);
                        }
                        break;
                    }
                }
            }
            string log = "Bank item groups: ";
            result.ForEach(item => log += (item + ", "));
            return result;
        }
    }
}

