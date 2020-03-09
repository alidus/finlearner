using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameModeType { FreePlay, Card}

[CreateAssetMenu(menuName = "ScriptableObjects/GameMode")]
public class GameMode : ScriptableObject
{
    public string title;
    public GameModeType type;
    public int money;
    public int mood;
    public float dayDuration;
    public int age;
}
