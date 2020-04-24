using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Cards : MonoBehaviour
{
    GameObject cardPrefab;
    Transform cardsGridPanel;
    Button backButton;
    GameManager gameManager;

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
        gameManager = GameManager.instance;
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
            var infoWrapper = card.transform.Find("InfoWrapper");
            var titleBackground = infoWrapper.Find("TitleBackground");
            titleBackground.GetComponent<Image>().color = Random.ColorHSV(0, 1, 0.85f, 0.95f, 0.30f, 0.40f, 0.40f, 0.50f);
            titleBackground.Find("Title").GetComponent<Text>().text = cardGM.Title;
            infoWrapper.Find("ScrollView").Find("Viewport").Find("Content").Find("Desc").GetComponent<Text>().text = cardGM.Description;
            infoWrapper.Find("Image").GetComponent<Image>().sprite = cardGM.Sprite;

            var completedIndicator = card.transform.Find("CompletedIndicator");
            completedIndicator.gameObject.SetActive(cardGM.IsCompleted);


            card.GetComponent<Button>().onClick.AddListener(
                    delegate { gameManager.StartGame(cardGM); }
                    );
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
