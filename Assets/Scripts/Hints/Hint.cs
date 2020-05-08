using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hint
{
    public event Action OnAccept;
    public event Action OnCancel;
    public HintView View;
    public HintParams HintParams;

    private string title;
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
    UnityEngine.Object viewPrefab;
    HintType HintType;
    Transform parent;

    public Hint(Transform hintWrapper, string title, string msg, HintParams hintParams, HintType hintType = HintType.Message)
    {
        HintType = hintType;
        parent = hintWrapper;
        SetupPrefab();
        HintParams = hintParams;
        this.Title = title;
        this.Message = msg;
    }

    void SetupPrefab()
    {
        switch (HintType)
        {
            case HintType.Message:
                viewPrefab = Resources.Load("Prefabs/Hints/MessageHintView");
                break;
            case HintType.Confirmation:
                viewPrefab = Resources.Load("Prefabs/Hints/ConfirmationHintView");
                break;
            default:
                viewPrefab = Resources.Load("Prefabs/Hints/MessageHintView");
                break;
        }
    }

    private void InstantiateView()
    {
        View = (GameObject.Instantiate(viewPrefab, parent) as GameObject).GetComponent<HintView>();
        View.Init(this);
    }

    public void Show()
    {
        if (View == null)
        {
            InstantiateView();
        }
        View.Show();
    }

    public void Hide()
    {
        View.Hide();
    }
}

public struct HintParams
{
    public bool IsGamePaused;
    public bool IsShadedBackground;

    public HintParams(bool isGamePaused = false, bool isShadedBackground = true)
    {
        IsGamePaused = isGamePaused;
        IsShadedBackground = isShadedBackground;
    }
}