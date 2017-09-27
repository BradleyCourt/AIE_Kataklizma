using UnityEngine;
using System.Collections;
using UnityEditor;



// This defines how the TagList should be drawn
// in the inspector, when inspecting a GameObject with
// a MonoBehaviour which uses the TagList attribute

[CustomPropertyDrawer(typeof(UnityTagAttribute))]
public class UnityTagDrawer : PropertyDrawer
{
    UnityTagAttribute TagList
    {
        get { return ((UnityTagAttribute)attribute); }
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

        property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
        //EditorGUI.PropertyField(position, property);
        

        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.EndProperty();
    }
}