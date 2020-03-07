using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameMode")]
public class GameMode : ScriptableObject
{
    public string title;
    public int money;
    public int mood;
    public float dayDuration;
    public int age;
}
