using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractHint : MonoBehaviour
{
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

    GameObject panel;

    public AbstractHint(string title, string msg, GameObject panel)
    {
        this.Title = title;
        this.Message = msg;
        this.panel = panel;

        
        panel.transform.Find("TitlePanel").GetComponentInChildren<Text>().text = Title;
        GameObject bodyPanel = panel.transform.Find("BodyPanel").gameObject;
        bodyPanel.GetComponentInChildren<Text>().text = Message;
        bodyPanel.transform.Find("MessagePanel").transform.Find("OkButtonPanel").transform.Find("OkButton").GetComponent<Button>().onClick.AddListener(delegate { });
        
    }

    public void Show()
    {
        panel.GetComponent<Animator>().Play("FadeInAnimation");
    }

    public void Hide()
    {
        panel.GetComponent<Animator>().Play("FadeOutAnimation");
    }
}
