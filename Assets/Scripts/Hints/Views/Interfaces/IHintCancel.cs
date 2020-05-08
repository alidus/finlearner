using UnityEngine;
using System.Collections;
using System;

public interface IHintCancel
{
    event Action OnCancel;
}
