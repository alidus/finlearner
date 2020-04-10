using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Milestones : MonoBehaviour
{
    GameMode gameMode;
    GameManager gameManager;
    GameDataManager gameDataManager;
    List<Milestone> milestones = new List<Milestone>();
    GameObject milestonesWrapper;

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
        foreach (GameCondition winCondition in gameMode.WinConditionsContainer)
        {
            GameObject milestoneGO = GameObject.Instantiate(Resources.Load("Prefabs/Milestones/Milestone"), milestonesWrapper.transform) as GameObject;
            Milestone milestone = milestoneGO.GetComponent<Milestone>();
            milestones.Add(milestone);
            var valueCondition = (ValueCondition)winCondition;
            if (valueCondition != null)
            {
                milestone.DisposeUpdateDelegate();
                switch (valueCondition.TargetVariable)
                {
                    case TargetVariable.Money:
                        milestone.updateMilestoneDelegate += delegate
                        {
                            milestone.textComponent.text = "Вы заработали " +
                            gameDataManager.Money.ToString() + "$ из " +
                            valueCondition.TargetFloatValue.ToString() + "$";
                        };
                        gameDataManager.OnMoneyValueChanged -= milestone.UpdateMilestone;
                        gameDataManager.OnMoneyValueChanged += milestone.UpdateMilestone;
                        break;
                    case TargetVariable.Mood:
                        milestone.updateMilestoneDelegate += delegate
                        {
                            milestone.textComponent.text = "Ваш уровень счастья " +
                            gameDataManager.Mood.ToString() + " из " +
                            valueCondition.TargetFloatValue.ToString();
                        };
                        gameDataManager.OnMoodValueChanged -= milestone.UpdateMilestone;
                        gameDataManager.OnMoodValueChanged += milestone.UpdateMilestone;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void UpdateGameMode()
    {
        gameMode = gameManager.GameMode;
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
