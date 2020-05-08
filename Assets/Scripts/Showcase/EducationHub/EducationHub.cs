using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class EducationHub : Showcase<EducationEntity, EducationHub>
{
    EducationHubViewFactory factory;
    List<ItemGroup<EducationEntity>> majorItemGroups = new List<ItemGroup<EducationEntity>>();
    Animator animator;
    private ObservableCollection<Certificate> certificates = new ObservableCollection<Certificate>();

    public ObservableCollection<Certificate> Certificates { get => certificates; set { certificates = value; }  }

    public List<ItemGroup<EducationEntity>> MajorItemGroups { get => majorItemGroups; set { majorItemGroups = value; } }

    public Action OnSelectedMajorItemGroupChanged;

    private ItemGroup<EducationEntity> selectedMajorItemGroup;
    public ItemGroup<EducationEntity> SelectedMajorItemGroup { get => selectedMajorItemGroup; set
        {
            if (selectedMajorItemGroup != value)
            {
                if (selectedMajorItemGroup != null)
                {
                    selectedMajorItemGroup.IsSelected = false;
                }
                if (value != null)
                {
                    value.IsSelected = true;
                }
                selectedMajorItemGroup = value;
                OnSelectedMajorItemGroupChanged?.Invoke();
            }
        }
    }

    private void OnEnable()
    {
        animator = GetComponent<Animator>();

        ItemDatabase.Clear();
        ItemDatabase = LoadAssets();

        // Setup groups
        if (ItemDatabase.Count > 0)
        {
            MajorItemGroups = FormMajorItemGroups();
            if (MajorItemGroups.Count > 0)
                SelectedMajorItemGroup = MajorItemGroups[0];
            foreach (var itemGroup in MajorItemGroups)
            {
                ItemGroups.AddRange(FormItemGroups(itemGroup));
            }

            if (ItemGroups.Count > 0)
                SelectedItemGroup = ItemGroups[0];
        }

        if (factory == null)
        {
            factory = new EducationHubViewFactory(
                this,
                Resources.Load("Prefabs/EducationHub/Views/EducationHubView"),
                Resources.Load("Prefabs/EducationHub/Views/EducationHubItemGroupListView"),
                Resources.Load("Prefabs/EducationHub/Views/EducationHubItemGroupView"),
                Resources.Load("Prefabs/EducationHub/Views/EducationHubItemListView"),
                Resources.Load("Prefabs/EducationHub/Views/EducationHubItemView"));
        }

        DestroyViews();
        RootView = factory.CreateRootView(this.transform);
    }

    protected override ItemDatabase<EducationEntity> LoadAssets()
    {
        ItemDatabase<EducationEntity> result = new ItemDatabase<EducationEntity>();
        foreach (EducationEntity eduDegree in Resources.LoadAll("ScriptableObjects/Education/Degrees"))
        {
            var degreeInstance = ScriptableObject.Instantiate(eduDegree) as EduDegree;
            degreeInstance.Init();
            result.Add(degreeInstance);
        }
        foreach (EducationEntity eduCourse in Resources.LoadAll("ScriptableObjects/Education/Courses"))
        {
            var courseInstance = ScriptableObject.Instantiate(eduCourse) as EduCourse;
            courseInstance.Init();
            result.Add(courseInstance);
        }

        return result;
    }



    public override void UpdateShowcase()
    {
        if (RootView != null)
        {
            RootView.UpdateView();
        }
    }

    protected List<ItemGroup<EducationEntity>> FormMajorItemGroups()
    {
        var result = new List<ItemGroup<EducationEntity>>();
        var degreesItemGroup = new ItemGroup<EduDegree>("Degrees");
        var coursesItemGroup = new ItemGroup<EduCourse>("Courses");
        foreach (EducationEntity educationEntity in ItemDatabase)
        {
            if (educationEntity is EduDegree)
                degreesItemGroup.Add(educationEntity as EduDegree);
            else if (educationEntity is EduCourse)
                coursesItemGroup.Add(educationEntity as EduCourse);
        }
        // TODO: improve algorythm, looks silly :(
        result.Add(new ItemGroup<EducationEntity>(degreesItemGroup.Items.ConvertAll(item => (EducationEntity)item), degreesItemGroup.Title));
        result.Add(new ItemGroup<EducationEntity>(coursesItemGroup.Items.ConvertAll(item => (EducationEntity)item), coursesItemGroup.Title));

        return result;
    }

    protected List<ItemGroup<EducationEntity>> FormItemGroups(ItemGroup<EducationEntity> targetItemGroup)
    {
        var result = new List<ItemGroup<EducationEntity>>();
        foreach (EducationEntity educationEntity in targetItemGroup)
        {
            var itemGroup = result.FirstOrDefault(group => group.Title == educationEntity.EducationDirection.ToString());
            if (itemGroup == null)
            {
                itemGroup = new ItemGroup<EducationEntity>(educationEntity.EducationDirection.ToString());
                itemGroup.ParentGroup = targetItemGroup;
                result.Add(itemGroup);
            }
            itemGroup.Add(educationEntity);
        }

        return result;
    }


    private void OnDisable()
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
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

    private void Update()
    {
        EducationTick();
    }

    private void EducationTick()
    {
        foreach (EducationEntity educationEntity in ItemDatabase)
        {
            if (!educationEntity.IsComplete && educationEntity.IsPurchased)
            {
                educationEntity.CurrentProgress += GameDataManager.instance.GetDeltaDay() / educationEntity.DurationInDays;
                if (educationEntity.CurrentProgress >= 1)
                {
                    educationEntity.Complete();
                }
            }
        }
    }
}
