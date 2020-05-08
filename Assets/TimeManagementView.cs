using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public class TimeManagementView : View
{
    GameDataManager gameDataManager;
    Transform timeConsumersWrapperTransform;
    Dictionary<ITimeConsumer, TimeConsumerView> timeConsumerViewsDict = new Dictionary<ITimeConsumer, TimeConsumerView>();

    private void OnEnable()
    {
        timeConsumersWrapperTransform = transform.Find("TimeConsumersWrapper");
    }

    // Start is called before the first frame update
    void Start()
    {
        gameDataManager = GameDataManager.instance;

        gameDataManager.TimeConsumers.CollectionChanged -= HandleTimeConsumersCollectionChanged;
        gameDataManager.TimeConsumers.CollectionChanged += HandleTimeConsumersCollectionChanged;
        DestroyAllTimeConsumerViews();
        UpdateView();
    }

    public override void UpdateView()
    {
        UpdateTimeConsumers();
    }

    float GetWrapperWidth()
    {
        return timeConsumersWrapperTransform.GetComponent<RectTransform>().rect.width;
    }

    void AddTimeConsumerView(ITimeConsumer timeConsumer)
    {
        if (!timeConsumerViewsDict.ContainsKey(timeConsumer))
        {
            TimeConsumerView timeConsumerView = GameObject.Instantiate(Resources.Load("Prefabs/TimeManagement/TimeConsumer") as GameObject, timeConsumersWrapperTransform).GetComponent<TimeConsumerView>();
            timeConsumerView.Init(timeConsumer);
            timeConsumerViewsDict[timeConsumer] = timeConsumerView;
            Debug.Log("Adding time consumer view " + timeConsumer.ToString());
            StartCoroutine(CalculateAndSetWidthOfTimeConsumerRectTransform(timeConsumerView.GetComponent<RectTransform>(), timeConsumer));
        }
    }

    IEnumerator CalculateAndSetWidthOfTimeConsumerRectTransform(RectTransform rectTransform, ITimeConsumer timeConsumer)
    {
        yield return null;
        float calculatedWidth = GetWrapperWidth() * timeConsumer.HoursOfWeekToConsume / GameDataManager.TOTAL_FREE_HOURS_IN_A_WEEK;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, calculatedWidth);
    }

    void RemoveTimeConsumerView(ITimeConsumer timeConsumer)
    {
        if (timeConsumerViewsDict.ContainsKey(timeConsumer))
        {
            Destroy(timeConsumerViewsDict[timeConsumer].gameObject);
            timeConsumerViewsDict.Remove(timeConsumer);
            Debug.Log("Removing time consumer view " + timeConsumer.ToString());

        }
    }

    private void OnDestroy()
    {
        gameDataManager.TimeConsumers.CollectionChanged -= HandleTimeConsumersCollectionChanged;
    }

    void HandleTimeConsumersCollectionChanged(System.Object sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateView();
    }


    private void UpdateTimeConsumers()
    {
        // Add SEs views for each SE presented in manager but missing view in statistics hub
        foreach (ITimeConsumer timeConsumer in gameDataManager.TimeConsumers)
        {
            if (!timeConsumerViewsDict.ContainsKey(timeConsumer))
            {
                AddTimeConsumerView(timeConsumer);
            }
        }
        // Remove SEs that are presented in statistics list but are missing in status effect manager
        foreach (ITimeConsumer timeConsumer in timeConsumerViewsDict.Keys.ToList().Except(gameDataManager.TimeConsumers))
        {
            RemoveTimeConsumerView(timeConsumer);
        }
    }

    void DestroyAllTimeConsumerViews()
    {
        for (int i = timeConsumersWrapperTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(timeConsumersWrapperTransform.GetChild(i).gameObject);
        }
    }

}
