using UnityEngine;
using UnityEditor;

public interface IViewImage
{
    Sprite Sprite { get; set; }

    void UpdateImage();
}