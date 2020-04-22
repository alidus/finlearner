using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class GameConditionsContainer : IEnumerable
{
    [SerializeField]
    private List<ValueCondition> valueConditions = new List<ValueCondition>();
    [SerializeField]
    private List<OwnCondition> ownConditions = new List<OwnCondition>();

    public int Count { get { return ValueConditions.Count + OwnConditions.Count; } }

    public List<ValueCondition> ValueConditions { get => valueConditions; set => valueConditions = value; }
    public List<OwnCondition> OwnConditions { get => ownConditions; set => ownConditions = value; }


    public void AddCondition(GameCondition condition)
    {
        if (condition is ValueCondition)
        {
            ValueConditions.Add((ValueCondition)condition);
        } else if (condition is OwnCondition)
        {
            OwnConditions.Add((OwnCondition)condition);
        }
    }

    public void RemoveCondition(GameCondition condition)
    {
        if (condition is ValueCondition)
        {
            ValueConditions.Remove((ValueCondition)condition);
        }
        else if (condition is OwnCondition)
        {
            OwnConditions.Remove((OwnCondition)condition);
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
            index -= ValueConditions.Count;
            if (index < OwnConditions.Count)
            {
                return OwnConditions[index];
            } else
            {
                return null;
            }
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
