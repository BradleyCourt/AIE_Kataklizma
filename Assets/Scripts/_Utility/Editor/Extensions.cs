using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static partial class Extensions {

    public static T GetBaseProperty<T>(this UnityEditor.SerializedProperty prop) {
        // Separate the steps it takes to get to this property
        string[] separatedPaths = prop.propertyPath.Split('.');

        // Go down to the root of this serialized property
        System.Object reflectionTarget = prop.serializedObject.targetObject as object;
        // Walk down the path to get the target objects
        foreach (var path in separatedPaths) {
            System.Reflection.FieldInfo fieldInfo = reflectionTarget.GetType().GetField(path);
            reflectionTarget = fieldInfo.GetValue(reflectionTarget);
        }
        return (T)reflectionTarget;
    }

}
