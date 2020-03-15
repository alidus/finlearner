using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStoreItemView
{

    StoreItem StoreItem { get; }

    string Title { get;  }
    Sprite Sprite { get;  }

    void Update();
}
