using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEditor;
using UnityEngine;


/// <summary>
/// The design-time (editor) parts of ValueCollection
/// </summary>
public partial class ValueCollection {

    [UnityEditor.CustomPropertyDrawer(typeof(Value))]
    public class PresetDrawer : UnityEditor.PropertyDrawer {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (property.isExpanded) {
                return 2 * EditorGUIUtility.singleLineHeight;
            } else {
                return EditorGUIUtility.singleLineHeight;
            }
        }

        /// <summary>
        /// Code sourced from: https://docs.unity3d.com/Manual/editor-PropertyDrawers.html
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            //base.OnGUI(position, property, label);

            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            label.text = ((ValueType)property.FindPropertyRelative("Type").intValue).ToString();


            // Draw foldout label
            //property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);

            Rect typeRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("Type"), GUIContent.none);

            position.x += EditorGUIUtility.labelWidth;
            position.width -= EditorGUIUtility.labelWidth;

            // Don't make child fields be indented
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var thirds = position;
            thirds.width /= 3.0f;
            thirds.height = EditorGUIUtility.singleLineHeight;// property.FindPropertyRelative("Base");

            
            Rect baseRect = new Rect(thirds.x, thirds.y, thirds.width, thirds.height);
            Rect modifierRect = new Rect(thirds.x + thirds.width, thirds.y, thirds.width, thirds.height);
            Rect derivedRect = new Rect(thirds.x + 2 * thirds.width, thirds.y, thirds.width, thirds.height);

            var value = new Value { Base = property.FindPropertyRelative("Base").floatValue, Modifier = property.FindPropertyRelative("Modifier").floatValue };

            var oldLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 18;

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(baseRect, property.FindPropertyRelative("Base"), new GUIContent(" B", "Base"));
            EditorGUI.PropertyField(modifierRect, property.FindPropertyRelative("Modifier"), new GUIContent(" M", "Modifier"));
            EditorGUI.LabelField(derivedRect, new GUIContent(" D", "Derived: Base * (1.0 + Modifier)\nREAD ONLY"), new GUIContent(value.Derived.ToString()), new GUIStyle(EditorStyles.numberField));

            EditorGUIUtility.labelWidth = oldLabelWidth;

            //if (property.isExpanded) {
            //    var typeRect = position;

            //    typeRect.y += EditorGUIUtility.singleLineHeight;
            //    typeRect.height = EditorGUIUtility.singleLineHeight;

            //    EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("Type"), GUIContent.none);
            //}

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}

