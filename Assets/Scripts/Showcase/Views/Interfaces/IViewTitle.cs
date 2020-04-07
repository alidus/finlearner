using UnityEngine;
using UnityEditor;

public interface IViewTitle
{
    string Title { get; set; }

    void UpdateTitle();
}