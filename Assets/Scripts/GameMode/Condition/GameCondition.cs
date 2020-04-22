using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public abstract class GameCondition
{
    public event UnityAction OnStateChanged;
    [SerializeField]
    private bool state;
    public bool State { get => state; set { state = value; OnStateChanged?.Invoke(); } }


    public GameCondition()
    {

    }

    public abstract void SubscribeToTargetDataChanges();
}
