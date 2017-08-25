﻿using UnityEngine;
using System.Collections;
using UnityEditor;

// This class defines the TagList attribute, so that
// it can be used in your regular MonoBehaviour scripts:

public class UnityLayerAttribute : PropertyAttribute
{

}

// This defines how the TagList should be drawn
// in the inspector, when inspecting a GameObject with
// a MonoBehaviour which uses the TagList attribute

[CustomPropertyDrawer(typeof(UnityLayerAttribute))]
public class UnityLayerDrawer : PropertyDrawer
{
    UnityLayerAttribute UnityLayer
    {
        get { return ((UnityLayerAttribute)attribute); }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded)
        {
            return 2 * EditorGUIUtility.singleLineHeight;
        }
        else
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        //position = EditorGUI.PrefixLabel(position, label);

        property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        //EditorGUI.PropertyField(position, property);
        

        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.EndProperty();
    }
}