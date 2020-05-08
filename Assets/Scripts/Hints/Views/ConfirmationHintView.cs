using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationHintView : HintView, IHintAccept, IHintCancel
{
    public event Action OnCancel;

    Button cancelButton;

    protected override void UpdateReferences()
    {
        base.UpdateReferences();
        cancelButton = controlPanel.Find("CancelButton").GetComponent<Button>();
    }

    protected override void InitButtonsActions()
    {
        base.InitButtonsActions();
        cancelButton.onClick.AddListener(delegate
        {
            OnCancel?.Invoke();
            Hide();
        });
    }
}
