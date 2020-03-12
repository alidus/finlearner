﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintsManager : MonoBehaviour
{
    public static HintsManager instance;

    // Prefabs
    GameObject hoveringPanelPrefab;
    GameObject uiCanvas;

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

        DontDestroyOnLoad(gameObject);

        UpdateReferences();
    }

    void UpdateReferences()
    {
        uiCanvas = GameObject.Find("UICanvas");
        hoveringPanelPrefab = Resources.Load<GameObject>("Prefabs/Hints/HoveringPanel");
    }

    public void ShowHint(string title, string msg, IHintPresenter presenter)
    {
        Hint hint = new Hint(title, msg, presenter);
        hint.Show();
    }
}
