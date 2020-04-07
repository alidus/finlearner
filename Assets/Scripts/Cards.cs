using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Cards : MonoBehaviour
{
    GameObject cardPrefab;
    Transform cardsGridPanel;
    Button backButton;

    private void OnEnable()
    {
        UpdateReferences();
        UpdateCardsList();
    }

    private void UpdateReferences()
    {
        cardsGridPanel = transform.Find("CardsGridPanel");
        backButton = transform.Find("BackButton")?.GetComponent<Button>();
    }

    private void OnDisable()
    {
        ClearCards();
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        backButton.onClick.AddListener(Hide);
    }

    void UpdateCardsList()
    {
        ClearCards();
        cardPrefab = Resources.Load("Prefabs/MainMenu/Card") as GameObject;

        List<GameMode> cards = Resources.LoadAll("ScriptableObjects/GameModes/Cards").ToList().ConvertAll(item => (GameMode)item);
        Debug.Log("Found " + cards.Count + " cards");
        foreach (GameMode cardGM in cards)
        {
            GameObject card = Instantiate(cardPrefab, cardsGridPanel.transform);

            card.transform.Find("Title").GetComponent<Text>().text = cardGM.Title;
        }
    }

    void ClearCards()
    {
        for (int i = cardsGridPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(cardsGridPanel.transform.GetChild(i).gameObject);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
