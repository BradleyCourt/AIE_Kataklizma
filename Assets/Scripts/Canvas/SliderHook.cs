using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canvas {
    public class SliderHook : MonoBehaviour {

        public Gameplay.EntityStats AttributeSource;

        private UnityEngine.UI.Slider Widget;

        public ValueType ValueSource;
        public ValueType ValueMaxSource;

        [Tooltip("When the Max value changes, should the old Max become the new Min?")]
        public bool MinFollowsMax;


        // Use this for initialization
        void Start() {
            Widget = GetComponent<UnityEngine.UI.Slider>();
            if (Widget == null) throw new System.ApplicationException(gameObject.name + " - SliderHook: Unable to locate required UI.Slider sibling.");

            if (AttributeSource == null) throw new System.ApplicationException(gameObject.name + " - SliderHook: AttributeSource cannot be null!");

            AttributeSource.ValueChanged += OnValueChanged;

            Widget.maxValue = AttributeSource[ValueMaxSource];
            Widget.value = AttributeSource[ValueSource];
        }

        private void OnValueChanged(Object source, ValueType type, ValueSubtype subtype, float oldValue) {

            if (type == ValueSource) {
                // Value Changed
                Widget.value = AttributeSource[ValueSource];
            } else if (type == ValueMaxSource) {
                // MAX Value Changed
                Widget.maxValue = AttributeSource[ValueMaxSource];
                if (MinFollowsMax) 
                    Widget.minValue = oldValue;

                Widget.value = AttributeSource[ValueSource];
            }
        }
        
    }
}