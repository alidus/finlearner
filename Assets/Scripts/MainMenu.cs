using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    GameManager gameManager;
    UIManager uiManager;

    Transform buttonsContainer;
    GameObject campaignButtonGO;
    GameObject freeplayButtonGO;
    GameObject settingsButtonGO;
    GameObject cardsPanel;
    Cards cards;


    private void OnEnable()
    {
        buttonsContainer = transform.Find("ButtonsContainer");
        if (buttonsContainer)
        {
            campaignButtonGO = buttonsContainer.Find("CampaignButton").gameObject;
            freeplayButtonGO = buttonsContainer.Find("FreePlayButton").gameObject;
            settingsButtonGO = buttonsContainer.Find("SettingsButton").gameObject;
        }

        cardsPanel = transform.Find("Cards").gameObject;
        if (cardsPanel)
        {
            cards = cardsPanel.GetComponent<Cards>();
        }
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        uiManager = UIManager.instance;

        if (campaignButtonGO)
        {
            Button campaignButton = campaignButtonGO.GetComponent<Button>();
            if (campaignButton)
            {
                campaignButton.onClick.RemoveAllListeners();
                campaignButton.onClick.AddListener(cards.Toggle);
            }
        }

        if (freeplayButtonGO)
        {
            Button freeplayButton = freeplayButtonGO.GetComponent<Button>();
            if (freeplayButton)
            {
                GameMode freeplayGM = Resources.Load("ScriptableObjects/GameModes/GM_Freeplay") as GameMode;
                freeplayButton.onClick.RemoveAllListeners();
                freeplayButton.onClick.AddListener(
                    delegate { gameManager.StartGame(freeplayGM); }
                    );
            }
        }

        if (settingsButtonGO)
        {
            Button settingsButton = settingsButtonGO.GetComponent<Button>();
            if (settingsButton)
            {
                settingsButton.onClick.RemoveAllListeners();
                settingsButton.onClick.AddListener(gameManager.GameSettings.Show);
            }
        }
    }
}
