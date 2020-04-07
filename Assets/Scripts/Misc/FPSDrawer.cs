using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDrawer : MonoBehaviour
{
    Text textComponent;

    private void Awake()
    {
        textComponent = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if (textComponent)
        {
            textComponent.text = ((int)Mathf.Round(1 / Time.deltaTime)).ToString() + " FPS";
        }
    }
}
