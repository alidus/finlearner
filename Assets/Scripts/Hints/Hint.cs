using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hint
{
    private string title;
    public IHintPresenter Presenter { private get; set; }
    public string Title
    {
        get { return title; }
        set { title = value; }
    }
    private string message;
    public string Message
    {
        get { return message; }
        set { message = value; }
    }

    public Hint(string title, string msg, IHintPresenter hintPresenter)
    {
        this.Title = title;
        this.Message = msg;
        this.Presenter = hintPresenter;
        this.Presenter.Message = Message;
        this.Presenter.Title = Title;
    }

    public void Show()
    {
        Presenter.Show();
    }

    public void Hide()
    {
        Presenter.Hide();
    }
}
