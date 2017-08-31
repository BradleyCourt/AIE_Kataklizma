using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
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

    [BitfieldMask(ShowCompoundValue = true)]
    public TestFlags MultiSelect;

}
