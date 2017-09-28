using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(BitfieldMaskAttribute))]
public class BitfieldMaskDrawer : PropertyDrawer {

    // Code adapted from: http://www.chronosapien.com/blog/broken-ice-enums-pt-2


    BitfieldMaskAttribute Options {
        get { return ((BitfieldMaskAttribute)attribute); }
    }

    //public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
    //    if (property.isExpanded) {
    //        return 2 * EditorGUIUtility.singleLineHeight;
    //    } else {
    //        return EditorGUIUtility.singleLineHeight;
    //    }
    //}

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

        // If multiselected, are selection's values different
        bool isMixedPrior = EditorGUI.showMixedValue;
        if (property.hasMultipleDifferentValues)
            EditorGUI.showMixedValue = true;

        // Unity's targetObjects order is backward: Find 'first' selected object
        SerializedProperty firstSelection = property.serializedObject.targetObjects.Select(m => new SerializedObject(m).FindProperty(property.propertyPath)).Last();

        EditorGUI.BeginChangeCheck();


        var enumType = GetEnumType();

        var oldUnityMask = EnumToUnityMask(enumType, firstSelection.intValue);

        // Handle the field for the first selection only.
        // Since we're assigning only one selectin's intValue to itself, this will never overwrite a value.
        var newUnityMask = EditorGUI.MaskField(position, label, oldUnityMask, GetDisplayableNames(enumType));

        // If the selection changed, apply it to all selections
        if (EditorGUI.EndChangeCheck()) {
            
            property.intValue = ParseUnityMaskChanges(enumType, oldUnityMask, newUnityMask); // Convert option index flags back to enum values

            //Debug.Log("BitfieldMask Changed: " + property.intValue + " [" + oldUnityMask + " > " + newUnityMask + "]");
        }

        EditorGUI.showMixedValue = isMixedPrior;

    }

    private Type GetEnumType() {
        Type type = fieldInfo.FieldType;

        if (!type.IsEnum)
            type = type.GetGenericArguments()[0];

        return type;
    }


    private string[] GetDisplayableNames(Type enumType) {
        var itemNames = Enum.GetNames(enumType);
        var itemValues = Enum.GetValues(enumType) as int[];

        var result = new List<string>(itemValues.Length);

        for (var i = 0; i < itemValues.Length; i++) {
            if (IsDisplayable(itemValues[i]))
                result.Add(itemNames[i]);
        }

        return result.ToArray();
    }

    private int[] GetDisplayableValues(Type enumType) {
        var itemNames = Enum.GetNames(enumType);
        var itemValues = Enum.GetValues(enumType) as int[];

        var result = new List<int>(itemValues.Length);

        for (var i = 0; i < itemValues.Length; i++) {
            if (IsDisplayable(itemValues[i]))
                result.Add(itemValues[i]);
        }

        return result.ToArray();
    }


    /// <summary>
    /// Test is Value should be displayed
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private bool IsDisplayable(int value) {
        if ((value & Options.AllowedValues) != value) // value (or part thereof) is not in Allowed Values
            return false;

        if (value == 0)  // If value is zero
            return Options.ShowZeroValue;

        if (((value & (value - 1)) == 0) || Options.ShowCompoundValue) // If value is not compound, or we should display compound values
            return true;

        return false;
    }

    /// <summary>
    /// Convert C# enum flags field to Unity Bit-Index field
    /// </summary>
    /// <param name="enumType"></param>
    /// <param name="enumMask"></param>
    /// <returns></returns>
    private int EnumToUnityMask(Type enumType, int enumMask) {
        var itemValues = GetDisplayableValues(enumType);

        int result = 0;

        for (var valueIndex = 0; valueIndex < itemValues.Length; valueIndex++) {                     // Foreach individual enam value
            if (itemValues[valueIndex] != 0 || Options.ShowZeroValue) {                              // If the value is not zero, or we should display zero values

                if ((enumMask & itemValues[valueIndex]) == itemValues[valueIndex]) {                 // If the value is in the given parameter
                    result |= 1 << valueIndex;                                                   // Set value index bit
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Convert Unity Bit-Index field to C# enum flags field (Respects indices of compound values)
    /// </summary>
    /// <param name="enumType"></param>
    /// <param name="oldUnityMask"></param>
    /// <param name="newUnityMask"></param>
    /// <returns></returns>
    private int ParseUnityMaskChanges(Type enumType, int oldUnityMask, int newUnityMask) {
        var itemValues = GetDisplayableValues(enumType) as int[];

        int switchOn = 0; // Selected values
        int switchOff = 0; // De-selected values

        for (var bitIndex = 0; bitIndex < itemValues.Length; bitIndex++) {
            if (itemValues[bitIndex] != 0 || Options.ShowZeroValue) {
                if ((newUnityMask & (1 << bitIndex)) != 0)
                    switchOn |= itemValues[bitIndex];
                else if ((oldUnityMask & (1 << bitIndex)) != 0)
                    switchOff |= itemValues[bitIndex];
            }
        }

        return switchOn & ~switchOff;
    }


}