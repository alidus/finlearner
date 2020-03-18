﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controls store logic
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    // Controllers, Managers
    private GameManager gameManager;
    private GameDataManager gameDataManager;
    private StatusEffectsManager statusEffectsController;
    private EnvironmentManager houseManager;
    private UIManager uiManager;

    // Events, delegates
    public delegate void StoreStateChangedAction();
    public event StoreStateChangedAction OnStoreStateChanged;



    public Store Store { get; set; }

    private StatusEffect dafaultStoreItemStatusEffect = new StatusEffect("Покупка в магазине", 20, StatusEffectType.Mood, StatusEffectFrequency.OneShot);
    

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

    public void UpdateReferences()
    {
        
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        gameManager = GameManager.instance;
        gameDataManager = GameDataManager.instance;
        statusEffectsController = StatusEffectsManager.instance;
        houseManager = EnvironmentManager.instance;
        uiManager = UIManager.instance;
        SceneManager.sceneLoaded += SceneLoadedHandling;
    }

    private void SceneLoadedHandling(Scene arg0, LoadSceneMode arg1)
    {
        UpdateReferences();
        Debug.Log(this.GetType().ToString() + "scene loaded handled");

    }

    public void InstantiateHomeStore(Transform storeContainerTransform)
    {
        uiManager = UIManager.instance;
        if (uiManager.storeContainer)
        {
            if (!storeContainerTransform.gameObject.GetComponent<Store>())
            {
                storeContainerTransform.gameObject.AddComponent<Store>();
            }
        }
    }
}
