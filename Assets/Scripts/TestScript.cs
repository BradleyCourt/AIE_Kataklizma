using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class TestScript : MonoBehaviour {

    [System.Flags]
    public enum TestFlags {
        Flag1 = 1,
        Flag2 = 2,
        Flag3 = 32,
        FlagCombo = Flag1 | Flag2,
    }


    public TestFlags SingleSelect;

    [BitfieldMask(typeof(TestFlags))]
    public TestFlags MultiSelect;

    public class BitfieldMaskAttribute : UnityEngine.PropertyAttribute {
        public Type EnumType;

        public BitfieldMaskAttribute(Type enumType) {
            EnumType = enumType;            
        }
    }

    [CustomPropertyDrawer(typeof(BitfieldMaskAttribute))]
    public class BitfieldMaskDrawer : PropertyDrawer {

        // Code stolen from http://wiki.unity3d.com/index.php/EnumFlagPropertyDrawer

        BitfieldMaskAttribute BitfieldMask {
            get { return ((BitfieldMaskAttribute)attribute); }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (property.isExpanded) {
                return 2 * EditorGUIUtility.singleLineHeight;
            } else {
                return EditorGUIUtility.singleLineHeight;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

            BitfieldMaskAttribute settings = (BitfieldMaskAttribute)attribute;
            Enum targetEnum = property.GetBaseProperty<Enum>();
            

            EditorGUI.BeginProperty(position, label, property);
            //Enum enumNew = EditorGUI.EnumMaskPopup(position, property.name, targetEnum);
            //property.intValue = (int)Convert.ChangeType(enumNew, targetEnum.GetType());

            property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
            
            EditorGUI.EndProperty();

            Debug.Log("BitfieldMask: " + property.intValue + " | " + ((TestFlags)property.intValue).ToString());
        }


    }
}
