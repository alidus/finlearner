using UnityEngine;
using System.Collections;
using System;

[Flags]
public enum GameConditionsGroupFlags { WinConditionsGroup = 1, LoseConditionGroup = 2 }
[System.Serializable]
public class GameConditionGroup
{
    public string Title { get => title; set => title = value; }
    public GameConditionsContainer GameConditionsContainer { get => gameConditionsContainer; set => gameConditionsContainer = value; }
    [SerializeField]
    private GameConditionsContainer gameConditionsContainer;
    [SerializeField]
    private string title = "CONDITION_GROUP_TITLE";
    [SerializeField]
    private GameConditionsGroupFlags flags;

    public int Count { get => gameConditionsContainer.Count; }
    public GameConditionsGroupFlags Flags { get => flags; set => flags = value; }

    public GameConditionGroup(string title, GameConditionsGroupFlags flags = 0)
    {
        Title = title;
        GameConditionsContainer = new GameConditionsContainer();
        Flags = flags;
    }

    public void AddCondition(GameCondition condition)
    {
        GameConditionsContainer.AddCondition(condition);
    }

    public void RemoveCondition(GameCondition condition)
    {
        GameConditionsContainer.RemoveCondition(condition);
    }
}
