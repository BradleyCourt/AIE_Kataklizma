using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


namespace Gameplay {

    [DisallowMultipleComponent]
    public class EntityAttributes : MonoBehaviour {

        public bool ShowDebug;

        #region " Stats "
        public event System.Action<Object, ValueType, ValueSubtype, float> ValueChanged;

        protected void RaiseValueChanged(ValueType type, ValueSubtype subtype, float oldValue) {
            if (ValueChanged != null) ValueChanged(this, type, subtype, oldValue);
        }

        [SerializeField]
        protected ValueCollection _Attributes;

        public float this[ValueType type, ValueSubtype subtype = ValueSubtype.Derived] {
            get { return _Attributes[type, subtype]; }
            set { _Attributes[type, subtype] = value; }
        }

        #endregion

        public bool IsAlive { get { return this[ValueType.Health] > 0; } }


        public EntityAttributes() {
            _Attributes = new ValueCollection();
            _Attributes.ValueChanged += OnStatsValueChanged;

        }

        // Use this for initialization
        void Start() {
            
        }

        private void OnStatsValueChanged(object arg1, ValueType arg2, ValueSubtype arg3, float arg4) {
            if ( ShowDebug )
                Debug.Log(gameObject.name + " - EntityStats: Value Changed: " + arg2.ToString() + ", " + arg3.ToString() + ", " + arg4.ToString("N2") + " -> " + _Attributes[arg2, arg3].ToString("N2"));
            RaiseValueChanged(arg2, arg3, arg4);
        }


        public void RemoveHealth(float Damage)
        {
            throw new System.NotImplementedException(gameObject.name + " - EntityAttributes::RemoveHealth(): This call is deprecated, use ApplyEffect instead");
        }

        public void ApplyEffects( IEnumerable<ValueCollection.Value> effects )
        {
            foreach (var effect in effects)
                ApplyEffect(effect.Type, effect.Derived);
        }

        public void ApplyEffect(ValueType type, float value) {
            switch (type) {
                case ValueType._NONE: break;
                case ValueType.Damage:
                    this[ValueType.Health] -= Mathf.Clamp(value - this[ValueType.DamageReduction], 0, this[ValueType.Health]);
                    break;
                case ValueType.Health:
                    this[ValueType.Health] += Mathf.Clamp(value, 0, this[ValueType.HealthMax] - this[ValueType.Health]);
                    break;
                default:
                    Debug.LogWarning(gameObject.name + " - EntityAttributes::ApplyEffect(): Unknown value type: " + type.ToString());
                    break;
            }
        }
    }
}