using UnityEngine;
using System.Collections;

public enum TimeConsumerCategory { Sleep, Job, Education }
public interface ITimeConsumer
{
    TimeConsumerCategory TimeConsumerCategory { get; set; }
    float HoursOfWeekToConsume { get; }
}
