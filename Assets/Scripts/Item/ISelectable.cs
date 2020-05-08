using UnityEngine;
using System.Collections;
using System;

public interface ISelectable
{
    bool IsSelected { get; set; }

    event Action OnSelectedStateChanged;
}
