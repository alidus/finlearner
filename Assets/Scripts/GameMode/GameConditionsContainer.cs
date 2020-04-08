using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public struct GameConditionsContainer : IEnumerable
{
    [SerializeField]
    private List<ValueCondition> valueConditions;

    public int Count { get { return ValueConditions.Count; } }

    public List<ValueCondition> ValueConditions { get => valueConditions; set => valueConditions = value; }

    public void Init()
    {
        if (ValueConditions == null)
            ValueConditions = new List<ValueCondition>();
    }

    public void AddCondition(GameCondition condition)
    {
        if (condition is ValueCondition)
        {
            ValueConditions.Add((ValueCondition)condition);
        }
    }

    public void RemoveCondition(GameCondition condition)
    {
        if (condition is ValueCondition)
        {
            ValueConditions.Remove((ValueCondition)condition);
        }
    }

    /// <summary>
    /// Goes by each group of GameConditions and if index is greater than size of group then shrink index by group size and go to next group
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameCondition GetConditionByIndex(int index)
    {
        if (index < ValueConditions.Count)
        {
            return ValueConditions[index];
        } else
        {
            return null;
        }
    }


    public IEnumerator GetEnumerator()
    {
        return new GameConditionsContainerEnumerator(this);
    }
}

public class GameConditionsContainerEnumerator : IEnumerator
{
    public object Current  => container.GetConditionByIndex(position);

    int position = -1;
    GameConditionsContainer container;

    public GameConditionsContainerEnumerator(GameConditionsContainer container)
    {
        this.container = container;
    }

    public bool MoveNext()
    {
        position++;
        return position < container.Count;
    }

    public void Reset()
    {
        position = -1;
    }
}
