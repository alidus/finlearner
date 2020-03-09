using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEventMapper : MonoBehaviour
{
    private GameManager gameManager;
    private GameController gameController;

    private Button storeButton;
    private Button infoPanelButton;
    private Button getCreditButtonTEST;

    private void Start()
    {
        gameManager = GameManager.instance;

        storeButton = GameObject.Find("StoreButton").GetComponent<Button>();
        infoPanelButton = GameObject.Find("InfoPanel").GetComponent<Button>();
        getCreditButtonTEST = GameObject.Find("GetCreditButton").GetComponent<Button>();

        storeButton.onClick.AddListener(gameManager.ToggleStoreMenu);
        infoPanelButton.onClick.AddListener(gameManager.ToggleModifiersInformation);
        getCreditButtonTEST.onClick.AddListener(gameManager.gameController.TakeTestCredit);
    }
}
