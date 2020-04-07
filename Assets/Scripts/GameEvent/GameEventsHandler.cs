using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public class GameEventsHandler : MonoBehaviour
{
    public static GameEventsHandler instance;

    // Managers, Controllers
    GameDataManager gameDataManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        UpdateReferences();
        DontDestroyOnLoad(gameObject);
    }

    private void UpdateReferences()
    {

    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        gameDataManager = GameDataManager.instance;
    }

    public void ExecuteEvent(GameEvent gameEvent, float delay = 0)
    {
        gameEvent.Execute(this);
    }

    public void ExecuteEventBetweenDates(GameEvent gameEvent, DateTime startDate, DateTime endDate)
    {
        int range = (endDate - startDate).Days;
        DateTime randomDate = startDate.AddDays(Random.Range(0, range));
        ExecuteEventAtDate(gameEvent, randomDate);
    }

    public void ExecuteEventAtDate(GameEvent gameEvent, DateTime dateTime)
    {
        StartCoroutine(ExecuteEventAtDateCoroutine(gameEvent, dateTime));
        Debug.Log("Событие " + gameEvent.Title + " будет вызвано " + dateTime.Date.ToString());
    }

    private IEnumerator ExecuteEventAtDateCoroutine(GameEvent gameEvent, DateTime dateTime)
    {
        while (gameDataManager.CurrentDateTime != dateTime)
        {
            yield return null;
        }
        gameEvent.Execute(this);
    }
}
