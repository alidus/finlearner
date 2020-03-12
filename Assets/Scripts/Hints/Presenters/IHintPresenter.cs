using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IHintPresenter
{
    string Title {get ; set;}
    string Message { get; set; }
    bool ShadeBackground { get; set; }
    bool PauseGame { get; set; }

    void Show();
    void Hide();
}
