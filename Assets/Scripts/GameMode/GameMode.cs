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

    public List<GameCondition> LoseConditions { get => loseConditions; set => loseConditions = value; }
    public List<GameCondition> WinConditions { get => winConditions; set => winConditions = value; }

    public event UnityAction OnWin;
    public event UnityAction OnLoose;

    public event UnityAction OnWinningConditionsChanged;
    public event UnityAction OnLoosingConditionsChanged;

    private List<GameCondition> winConditions = new List<GameCondition>();
    private List<GameCondition> loseConditions = new List<GameCondition>();

    private void OnEnable()
    {
        WinConditions.Clear();
        LoseConditions.Clear();

        AddLoseCondition(new ValueCondition(GameDataManager.instance));
    }

    public void AddCondition(GameCondition condition, GameConditionGroup group)
    {
        switch (group)
        {
            case GameConditionGroup.Win:
                AddWinCondition(condition);
                break;
            case GameConditionGroup.Lose:
                AddLoseCondition(condition);
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
                RemoveWinCondition(condition);
                break;
            case GameConditionGroup.Lose:
                RemoveLoseCondition(condition);
                break;
            default:
                break;
        }
    }

    public void AddWinCondition(GameCondition gameCondition)
    {
        WinConditions.Add(gameCondition);
        OnWinningConditionsChanged?.Invoke();
    }
    public void RemoveWinCondition(GameCondition gameCondition)
    {
        WinConditions.Remove(gameCondition);
        OnWinningConditionsChanged?.Invoke();
    }
    public void AddLoseCondition(GameCondition gameCondition)
    {
        LoseConditions.Add(gameCondition);
        OnLoosingConditionsChanged?.Invoke();
    }
    public void RemoveLoseCondition(GameCondition gameCondition)
    {
        LoseConditions.Remove(gameCondition);
        OnLoosingConditionsChanged?.Invoke();
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


