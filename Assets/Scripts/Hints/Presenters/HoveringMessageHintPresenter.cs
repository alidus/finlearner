using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoveringMessageHintPresenter : AbstractPresenter
{
    public HoveringMessageHintPresenter(bool shadeBackground = false, bool pauseGame = false) : base(shadeBackground, pauseGame)
    {

    }

    public override void SetPrefab()
    {
        hintPrefab = Resources.Load<GameObject>("Prefabs/Hints/HoveringMessage");
    }
}
