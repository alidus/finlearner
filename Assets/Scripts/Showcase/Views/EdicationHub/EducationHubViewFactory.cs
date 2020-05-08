using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EducationHubViewFactory : DefaultShowcaseViewFactory<EducationEntity, EducationHub>
{
    new EducationHub showcase;

    protected View majorItemGroupListView;


    public EducationHubViewFactory(Showcase<EducationEntity, EducationHub> showcase, Object rootViewPrefab, Object itemGroupListViewPrefab, Object itemGroupViewPrefab, Object itemListViewPrefab, Object itemViewPrefab) : base(showcase, rootViewPrefab, itemGroupListViewPrefab, itemGroupViewPrefab, itemListViewPrefab, itemViewPrefab)
    {
        this.showcase = showcase as EducationHub;
    }

    public override View CreateRootView(Transform parentTransform)
    {
        Console.Print("___Start building education hub___");
        rootView = GameObject.Instantiate(rootViewPrefab as GameObject, parentTransform).GetComponent<EducationHubView>();
        majorItemGroupListView = CreateMajorItemGroupListView(rootView.transform);
        itemGroupListView = CreateItemGroupListView(rootView.transform);
        itemListView = CreateItemListView(rootView.transform);
        UpdateItemListView();
        showcase.OnSelectedItemGroupChanged -= UpdateItemListView;
        showcase.OnSelectedItemGroupChanged += UpdateItemListView;
        rootView.UpdateView();
        return rootView;
    }

    public View CreateMajorItemGroupListView(Transform parentTransform)
    {
        Console.Print("_Creating education hub major item group list view");
        DefaultItemGroupListView educationHubItemGroupListView = GameObject.Instantiate(itemGroupListViewPrefab as GameObject, parentTransform).GetComponent<DefaultItemGroupListView>();
        educationHubItemGroupListView.Init();
        foreach (ItemGroup<EducationEntity> itemGroup in showcase.MajorItemGroups)
        {
            CreateMajorItemGroupView(itemGroup, educationHubItemGroupListView.transform);
        }
        educationHubItemGroupListView.UpdateView();
        return educationHubItemGroupListView;
    }

    public virtual View CreateMajorItemGroupView(ItemGroup<EducationEntity> majorItemGroup, Transform parentTransform)
    {
        DefaultItemGroupView<EducationEntity> educationHubMajorItemGroupView = GameObject.Instantiate(itemGroupViewPrefab as GameObject, parentTransform).GetComponent<DefaultItemGroupView<EducationEntity>>();
        educationHubMajorItemGroupView.OnClick.AddListener(delegate {
            showcase.SelectedMajorItemGroup = majorItemGroup;
            // Change selected item group to one with the same name as previous, otherwise select first one
            // TODO: clean this
            var childGroups = showcase.ItemGroups.FindAll(group => group.ParentGroup == majorItemGroup);
            if (showcase.SelectedItemGroup != null)
            {
                var newSelectedMajorGroupChildGroupWithSameName = childGroups.FirstOrDefault(group => group.Title == showcase.SelectedItemGroup.Title);
                if (newSelectedMajorGroupChildGroupWithSameName != null)
                {
                    showcase.SelectedItemGroup = newSelectedMajorGroupChildGroupWithSameName;
                }
                else
                {
                    showcase.SelectedItemGroup = childGroups.Count > 0 ? childGroups[0] : null;
                }
            } else
            {
                showcase.SelectedItemGroup = childGroups.Count > 0 ? childGroups[0] : null;
            }

            RefreshItemGroupListView(itemGroupListView as DefaultItemGroupListView);
        });
        educationHubMajorItemGroupView.Init(majorItemGroup);

        educationHubMajorItemGroupView.UpdateView();
        return educationHubMajorItemGroupView;
    }

    public override View CreateItemGroupListView(Transform parentTransform)
    {
        Console.Print("_Creating education hub item group list view");

        DefaultItemGroupListView storeItemGroupListView = GameObject.Instantiate(itemGroupListViewPrefab as GameObject, parentTransform).GetComponent<DefaultItemGroupListView>();
        RefreshItemGroupListView(storeItemGroupListView);
        return storeItemGroupListView;
    }

    void RefreshItemGroupListView(DefaultItemGroupListView itemGroupListView)
    {
        Console.Print("_Refreshing education hub item group list view");

        itemGroupListView.DestroyChildren();
        foreach (ItemGroup<EducationEntity> group in showcase.ItemGroups)
        {
            if (group.ParentGroup == showcase.SelectedMajorItemGroup)
                CreateItemGroupView(group, itemGroupListView.transform);
        }
        itemGroupListView.UpdateView();
    }

}
