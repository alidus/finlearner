using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodPanelView : View
{
    Animator animator;
    Text valueText;

    public int Value { get; set; }

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        valueText = transform.Find("Text").GetComponent<Text>();
    }

    public override void UpdateView()
    {
        UpdateValue();
    }

    public void PlayAddAnim()
    {
        animator.SetTrigger("Add");
    }

    public void PlaySubAnim()
    {
        animator.SetTrigger("Sub");

    }

    private void UpdateValue()
    {
        valueText.text = Value.ToString();
    }
}
