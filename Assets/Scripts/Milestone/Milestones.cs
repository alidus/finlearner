using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Milestones : MonoBehaviour
{
    public static Milestones instance;
    GameMode gameMode;
    GameManager gameManager;
    GameDataManager gameDataManager;
    List<Milestone> milestones = new List<Milestone>();
    GameObject milestonesWrapper;

    public Action OnGameWin;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        milestonesWrapper = transform.Find("ScrollViewWrapper").Find("ScrollView").Find("Viewport").Find("MilestonesWrapper").gameObject;
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        gameDataManager = GameDataManager.instance;

        UpdateGameMode();
        InitMilestones();
        UpdateMilestones();
    }

    void InitMilestones()
    {
        DestroyMilestones();
        milestones.Clear();
        foreach (GameConditionGroup winGroup in gameMode.GetGameConditionGroupsWithFlags(GameConditionsGroupFlags.WinConditionsGroup))
        {
            foreach (GameCondition winCondition in winGroup.GameConditionsContainer)
            {
                CreateMilestone(winCondition);
            }
        }
    }

    void CreateMilestone(GameCondition condition)
    {
        Milestone milestone = null;
        if (condition is ValueCondition)
        {
            milestone = CreateValueMilestone((ValueCondition)condition);
        } else if (condition is OwnCondition)
        {
            milestone = CreateOwnMilestone((OwnCondition)condition);
        }
        if (milestone != null)
        {
            milestone.SubscribeToConditionChanges();
            milestone.OnStateChanged += CheckForGameStateChanged;
            milestones.Add(milestone);
        }
    }

    Milestone CreateValueMilestone(ValueCondition valueCondition)
    {
        Milestone milestone = (GameObject.Instantiate(Resources.Load("Prefabs/Milestones/Milestone"), milestonesWrapper.transform) as GameObject).GetComponent<Milestone>();
        milestone.GameCondition = valueCondition;
        return milestone;
    }

    OwnMilestone CreateOwnMilestone(OwnCondition ownCondition)
    {
        OwnMilestone milestone = (GameObject.Instantiate(Resources.Load("Prefabs/Milestones/OwnMilestone"), milestonesWrapper.transform) as GameObject).GetComponent<OwnMilestone>();
        milestone.OwnCondition = ownCondition;
        return milestone;
    }

    void UpdateGameMode()
    {
        gameMode = gameManager.GameMode;
    }

    void CheckForGameStateChanged()
    {
        if (!milestones.Exists(ms => ms.GetState() == false))
        {
            // All milestones completed
            OnGameWin?.Invoke();
        }
    }

    public void UpdateMilestones()
    {
        foreach (Milestone milestone in milestones)
        {
            milestone.UpdateMilestone();
        }
    }

    void DestroyMilestones()
    {
        for (int i = milestonesWrapper.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(milestonesWrapper.transform.GetChild(i).gameObject);
        }
    }
}
