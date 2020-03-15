using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbstractStoreItemBehaviour : IStoreItemBahaviour
{
    [SerializeField]
    public string meta;
    public virtual void Execute() { }
}
