using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.Button;



[System.Serializable]
public class Item
{
    [SerializeField]
    private int id;
    public int Id { get => id; set => id = value; }
    [SerializeField]
    private string title;
    public string Title
    {
        get { return title; }
        set { title = value; }
    }
    [SerializeField]
    private string desc;
    public string Desc
    {
        get { return desc; }
        set { desc = value; }
    }
    [SerializeField]
    private int price;
    public int Price
    {
        get { return price; }
        set { price = value; }
    }

}
