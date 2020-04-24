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
    private string description;
    public string Description { get => description; set => description = value; }

    [SerializeField]
    private Sprite sprite;
    public Sprite Sprite { get => sprite; set => sprite = value; }

    [SerializeField]
    private bool isCompleted;
    public bool IsCompleted { get => isCompleted; set => isCompleted = value; }


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

    public List<GameConditionGroup> GameConditionGroups { get => gameConditionGroups; }

    public event UnityAction OnWin;
    public event UnityAction OnLoose;

    public event UnityAction OnWinningConditionsChanged;
    public event UnityAction OnLoosingConditionsChanged;
    [SerializeField]
    private List<GameConditionGroup> gameConditionGroups = new List<GameConditionGroup>();
    private void OnEnable()
    {

    }

    public void SubscribeConditions()
    {
        foreach (GameConditionGroup group in gameConditionGroups)
        {
            foreach (GameCondition condition in group.GameConditionsContainer)
            {
                condition.SubscribeToTargetDataChanges();
            }
        }
    }

    public void ClearGameConditionGroups()
    {
        gameConditionGroups.Clear();
    }

    public void AddConditionGroup(GameConditionGroup group)
    {
        gameConditionGroups.Add(group);
    }

    public void RemoveConditionGroup(GameConditionGroup group)
    {
        gameConditionGroups.Remove(group);
    }

    public void AddCondition(GameCondition condition, GameConditionGroup group)
    {
        group.AddCondition(condition);
    }

    public void RemoveCondition(GameCondition condition, GameConditionGroup group) 
    {
        group.RemoveCondition(condition);
    }

    protected virtual void Win()
    {
        Debug.Log("You WIN!!");
        OnWin();
    }

    public virtual void SetupLooseCondition(GameDataManager gameDataManager)
    {

    }

    public List<GameConditionGroup> GetGameConditionGroupsWithFlags(GameConditionsGroupFlags flags)
    {
        List<GameConditionGroup> result = new List<GameConditionGroup>();
        foreach (GameConditionGroup group in GameConditionGroups)
        {
            if (group.Flags.HasFlag(flags))
                result.Add(group);
        }

        return result;
    }

    protected virtual void Loose()
    {
        Debug.Log("You lost :(");
        OnLoose();
    }
}


