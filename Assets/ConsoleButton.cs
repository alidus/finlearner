using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleButton : MonoBehaviour
{
    Button buttonComponent;

    private void OnEnable()
    {
        buttonComponent = GetComponent<Button>();
    }
    void Start()
    {
        buttonComponent.onClick.AddListener(Console.Toggle);
    }
}
