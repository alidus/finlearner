using UnityEngine;
using UnityEditor;

/// <summary>
/// Base abstract class for all view in game
/// </summary>
public abstract class View : MonoBehaviour
{
    public abstract void UpdateView();

    public void DestroyChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}