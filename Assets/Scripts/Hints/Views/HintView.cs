using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HintView : View, IHintAccept
{
    public string Title { get; set; }
    public string Message { get; set; }

    private GameObject shadedBackground;
    protected Transform controlPanel;
    HintParams hintParams;

    Text titleText;
    Text messageText;
    Button okButton;
    Animator animator;
    public event Action OnAccept;

    public void Init(Hint hint)
    {
        this.hintParams = hint.HintParams;
        Title = hint.Title;
        Message = hint.Message;
        UpdateView();
    }

    public override void UpdateView()
    {
        UpdateTitle();
        UpdateMessage();
        UpdateBackground();
    }

    private void UpdateMessage()
    {
        messageText.text = Message;
    }

    private void UpdateTitle()
    {
        titleText.text = Title;
    }

    private void UpdateBackground()
    {
        if (hintParams.IsShadedBackground)
        {
            if (shadedBackground == null)
            {

            }
        }
    }

    private void OnEnable()
    {
        UpdateReferences();
        InitButtonsActions();
    }

    protected virtual void UpdateReferences()
    {
        shadedBackground = Instantiate<GameObject>(Resources.Load("Prefabs/Hints/HintBackground") as GameObject, transform.parent);
        shadedBackground.transform.SetSiblingIndex(transform.GetSiblingIndex());
        titleText = transform.Find("TitlePanel").Find("TitleText").GetComponent<Text>();
        var bodyPanel = transform.Find("Body");
        var messagePanel = bodyPanel.Find("MessagePanel");
        messageText = messagePanel.Find("MessageText").GetComponent<Text>();
        controlPanel = messagePanel.Find("ControlPanel");
        okButton = controlPanel.Find("OkButton").GetComponent<Button>();
        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        Console.Print("Hint '" + Title + "' was created");
    }

    protected virtual void InitButtonsActions()
    {
        okButton.onClick.AddListener(delegate
        {
            OnAccept?.Invoke();
            Hide();
        });
    }

    public void Show()
    {
        animator.SetBool("IsShown", true);
        if (hintParams.IsShadedBackground)
        {
            shadedBackground.GetComponent<Animator>().SetBool("IsShown", true);
        }

        if (hintParams.IsGamePaused)
        {
            // Pause the game
            Time.timeScale = 0;
        }
    }

    public void Hide()
    {
        animator.SetBool("IsShown", false);
        if (hintParams.IsShadedBackground)
        {
            shadedBackground.GetComponent<Animator>().SetBool("IsShown", false);
        }

        Time.timeScale = 1;
        Console.Print("Destroying hint '" + Title + "' in " + animator.GetCurrentAnimatorStateInfo(0).length.ToString() + " seconds");

        GameObject.Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        GameObject.Destroy(shadedBackground, shadedBackground.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
}
