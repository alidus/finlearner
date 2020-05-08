using UnityEngine;
using System.Collections;

public class TimeConsumer : Item, ITimeConsumer
{
    public float HoursOfWeekToConsume { get; set; }
    public TimeConsumerCategory TimeConsumerCategory { get; set; }
}
