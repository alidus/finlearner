using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobExchangeViewFactory<T> : DefaultShowcaseViewFactory<T> where T : Job
{
    public JobExchangeViewFactory(Showcase<T> showcase, Object rootViewPrefab, Object itemGroupListViewPrefab, Object itemGroupViewPrefab, Object itemListViewPrefab, Object itemViewPrefab) : base(showcase, rootViewPrefab, itemGroupListViewPrefab, itemGroupViewPrefab, itemListViewPrefab, itemViewPrefab)
    {
    }
}
