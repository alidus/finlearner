using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemPurchaseBehaviour))]
public class ItemPurchaseBehaviourEditor : Editor
{
    SerializedObject behaviour;
    SerializedProperty price;
    private void OnEnable()
    {
        price = behaviour.FindProperty("price");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.ObjectField(price);
        EditorGUILayout.LabelField("Hello there");
        serializedObject.ApplyModifiedProperties();
        
    }
}
