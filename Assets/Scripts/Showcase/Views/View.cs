using UnityEngine;
using UnityEditor;

public abstract class View : MonoBehaviour
{
    public abstract void UpdateView();

    protected void DestroyChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}