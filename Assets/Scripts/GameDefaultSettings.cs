using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameModeType { FreePlay, Card}

[CreateAssetMenu(menuName = "ScriptableObjects/GameMode")]
public class GameDefaultSettings : ScriptableObject
{
    public string title;
    public GameModeType type;
    public int money;
    public int mood;
    public int age;
    public float hoursPerSecond;
}
