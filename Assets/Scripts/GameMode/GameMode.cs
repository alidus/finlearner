using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameModeType { FreePlay, Card}
public enum GameGoalType { GetValue }

[CreateAssetMenu(menuName = "SO/GameMode", fileName = "GameMode")]
public class GameMode : ScriptableObject
{
    [SerializeField]
    private string title;
    public string Title { get => title; set => title = value; }

    [SerializeField]
    private GameModeType type;
    public GameModeType Type
    {
        get { return type; }
        set { type = value; }
    }

    [SerializeField]
    private GameGoalType goalType;
    public GameGoalType GoalType
    {
        get { return goalType; }
        set { goalType = value; }
    }

    [SerializeField]
    private float money;
    public float Money
    {
        get { return money; }
        set { money = value; }
    }

    [SerializeField]
    private float mood;
    public float Mood
    {
        get { return mood; }
        set { mood = value; }
    }

    [SerializeField]
    private int age;
    public int Age
    {
        get { return age; }
        set { age = value; }
    }

    public GameConditionsContainer LoseConditionsContainer { get => loseConditionsContainer; set => loseConditionsContainer = value; }
    public GameConditionsContainer WinConditionsContainer { get => winConditionsContainer; set => winConditionsContainer = value; }

    public event UnityAction OnWin;
    public event UnityAction OnLoose;

    public event UnityAction OnWinningConditionsChanged;
    public event UnityAction OnLoosingConditionsChanged;
    [SerializeField]
    private GameConditionsContainer winConditionsContainer;
    [SerializeField]
    private GameConditionsContainer loseConditionsContainer;

    private void OnEnable()
    {
        winConditionsContainer.Init();
        loseConditionsContainer.Init();
    }

    public void AddCondition(GameCondition condition, GameConditionGroup group)
    {
        switch (group)
        {
            case GameConditionGroup.Win:
                winConditionsContainer.AddCondition(condition);
                break;
            case GameConditionGroup.Lose:
                loseConditionsContainer.AddCondition(condition);
                break;
            default:
                break;
        }
    }

    public void RemoveCondition(GameCondition condition, GameConditionGroup group) 
    {
        switch (group)
        {
            case GameConditionGroup.Win:
                winConditionsContainer.RemoveCondition(condition);
                break;
            case GameConditionGroup.Lose:
                loseConditionsContainer.RemoveCondition(condition);
                break;
            default:
                break;
        }
    }




    public virtual void SetupWinCondition(GameDataManager gameDataManager)
    {
        
    }

    protected virtual void Win()
    {
        Debug.Log("You WIN!!");
        OnWin();
    }

    public virtual void SetupLooseCondition(GameDataManager gameDataManager)
    {

    }

    protected virtual void Loose()
    {
        Debug.Log("You lost :(");
        OnLoose();
    }
}


