using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameConditionGroup { Win, Lose }


[System.Serializable]
public class GameCondition
{
    protected GameDataManager gameDataManager;
    public event UnityAction OnStateChanged;
    [SerializeField]
    private bool state;
    public bool State { get => state; set { state = value; OnStateChanged(); } }


    public GameCondition(GameDataManager gameDataManager)
    {
        this.gameDataManager = gameDataManager;
    }
}
