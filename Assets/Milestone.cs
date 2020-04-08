using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Milestone : MonoBehaviour
{
    public UnityAction updateMilestoneDelegate;
    public Text textComponent;

    private void OnEnable()
    {
        textComponent = transform.Find("Text").GetComponent<Text>();
    }

    public void DisposeUpdateDelegate()
    {
        updateMilestoneDelegate = null;
    }

    public void UpdateMilestone()
    {
        updateMilestoneDelegate?.Invoke();
    }
}
