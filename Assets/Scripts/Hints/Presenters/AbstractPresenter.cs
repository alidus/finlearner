using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractPresenter : IHintPresenter
{
    public string Title { get; set; }
    public string Message { get; set; }
    public bool ShadeBackground { get; set; }
    public bool PauseGame { get; set; }

    protected GameObject hintPrefab;
    private GameObject uiCanvas;
    private GameObject hintPanel;
    private GameObject shadeBackgroundPanel;

    public AbstractPresenter(bool shadeBackground = false, bool pauseGame = false)
    {
        SetPrefab();
        uiCanvas = GameObject.Find("UICanvas");
        ShadeBackground = shadeBackground;
        PauseGame = pauseGame;
    }
    public abstract void SetPrefab();

    public void Show()
    {
        if (hintPrefab && uiCanvas)
        {
            if (ShadeBackground)
            {
                // Create background with transparent dark image sized to full screen
                shadeBackgroundPanel = new GameObject("ShadeBackgroundPanel");
                shadeBackgroundPanel.AddComponent<CanvasRenderer>();
                Image shadeBackgroundImageComponent = shadeBackgroundPanel.AddComponent<Image>();
                shadeBackgroundImageComponent.color = new Color(0, 0, 0, 0.5f);
                shadeBackgroundPanel.transform.SetParent(uiCanvas.transform);
                RectTransform shadeBackgroundRectTransform = shadeBackgroundPanel.GetComponent<RectTransform>();
                CanvasGroup shadeBackgroundCanvasGroup = shadeBackgroundPanel.AddComponent<CanvasGroup>();
                Animator shadeBackgroundAnimator = shadeBackgroundPanel.AddComponent<Animator>();
                shadeBackgroundAnimator.runtimeAnimatorController = GameObject.Instantiate<RuntimeAnimatorController>(Resources.Load("AnimationControllers/HoveringPanel") as RuntimeAnimatorController);
                shadeBackgroundAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
                shadeBackgroundRectTransform.anchorMin = Vector2.zero;
                shadeBackgroundRectTransform.anchorMax = Vector2.one;
                shadeBackgroundRectTransform.sizeDelta = Vector2.zero;
                shadeBackgroundRectTransform.localScale = Vector2.one;
                shadeBackgroundAnimator.Play("FadeInAnimation");
            }
            hintPanel = GameObject.Instantiate(hintPrefab, uiCanvas.transform);
            hintPanel.transform.Find("TitlePanel").GetComponentInChildren<Text>().text = Title;
            GameObject bodyPanel = hintPanel.transform.Find("BodyPanel").gameObject;
            bodyPanel.GetComponentInChildren<Text>().text = Message;
            bodyPanel.transform.Find("MessagePanel").transform.Find("OkButtonPanel").transform.Find("OkButton").GetComponent<Button>().onClick.AddListener(delegate { Hide(); });
            hintPanel.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
            hintPanel.GetComponent<Animator>().Play("FadeInAnimation");

            if (PauseGame)
            {
                // Pause the game
                Time.timeScale = 0;
            }



        }
    }

    public void Hide()
    {
        hintPanel.GetComponent<Animator>().Play("FadeOutAnimation");
        if (shadeBackgroundPanel)
        {
            shadeBackgroundPanel.GetComponent<Animator>().Play("FadeOutAnimation");
        }

        Time.timeScale = 1;
        GameObject.Destroy(hintPanel, hintPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        GameObject.Destroy(shadeBackgroundPanel, hintPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
}
