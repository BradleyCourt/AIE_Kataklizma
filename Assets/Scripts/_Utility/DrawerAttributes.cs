using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BitfieldMaskAttribute : PropertyAttribute {
    public int AllowedValues = ~0;
    public bool ShowZeroValue = true;
    public bool ShowCompoundValue = true;
}

public class UnityLayerAttribute : PropertyAttribute {

}

// This class defines the TagList attribute, so that
// it can be used in your regular MonoBehaviour scripts:

public class UnityTagAttribute : PropertyAttribute {

}