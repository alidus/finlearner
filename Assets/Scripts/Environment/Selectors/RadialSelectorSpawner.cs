using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RadialSelectorSpawner : MonoBehaviour
{
    public static RadialSelectorSpawner instance;

    private RadialSelector activeSelector;


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
    }

    private void Start()
    {
        Init();
    }


    public void Init()
    {
        SceneManager.sceneLoaded += SceneLoadedHandling;
    }

    private void SceneLoadedHandling(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log(this.GetType().ToString() + "scene loaded handled");
    }

    public RadialSelector SpawnRadialSelector(PlaceableObject placeableObject)
    {
        RadialSelector radialSelector = (Instantiate(Resources.Load("Prefabs/HUD/RadialSelector"), transform) as GameObject).GetComponent<RadialSelector>();
        radialSelector.placeableObject = placeableObject;
        if (activeSelector)
        {
            activeSelector.HideAndDestroy();
        }

        switch (placeableObject)
        {
            case PlaceableFurniture _:

                break;

            default:
                break;
        }

        radialSelector.Init(placeableObject);
        activeSelector = radialSelector;
        return radialSelector;
    }

}
