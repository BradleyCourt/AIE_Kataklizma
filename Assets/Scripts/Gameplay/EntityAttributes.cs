using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gameplay {

    [DisallowMultipleComponent]
    public class EntityAttributes : MonoBehaviour {

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
            RaiseValueChanged(arg2, arg3, arg4);
        }


        public void RemoveHealth(float Damage)
        {
            throw new System.NotImplementedException(gameObject.name + " - EntityAttributes::RemoveHealth(): This call is deprecated, use ApplyEffect instead");
        }



        public List<ValueCollection.Value> GetEffects(List<ValueType> types) {
            var results = new List<ValueCollection.Value>();
            var typesFound = new List<ValueType>();

            foreach (var value in _Attributes._Values) {
                if (types.Contains(value.Type)) {
                    results.Add(value);
                    typesFound.Add(value.Type);
                }
            }

#if DEBUG
            var excepted = types.Except(typesFound).ToList();
            if (excepted.Count != 0)
                Debug.LogWarning(gameObject.name + " - EntityAttributes.GetEffects(): Unable to find all requested types [" + string.Join(",", excepted.Select(m => m.ToString()).ToArray()) + "]");

#endif
            return results;
        }


        #region " Apply Effects "
        /// <summary>
        /// 
        /// </summary>
        /// <param name="effects"></param>
        public void ApplyEffects(IEnumerable<ValueCollection.Value> effects) {
            foreach (var effect in effects)
                ApplyEffect(effect);
        }

        public void ApplyEffect(ValueCollection.Value value) {

            switch (value.Type) {
                case ValueType._NONE: break;
                case ValueType.Damage:
                    this[ValueType.Health] -= Mathf.Clamp(value.Derived - this[ValueType.DamageReduction], 0, this[ValueType.Health]);
                    break;
                case ValueType.Health:
                    this[ValueType.Health] += Mathf.Clamp(value.Derived, 0, this[ValueType.HealthMax] - this[ValueType.Health]);
                    break;
                case ValueType.ContactDamage:
                    ApplyEffect(ValueType.Damage, value.Derived);
                    break;
                case ValueType.ContactDamageBonus:
                    this[ValueType.ContactDamage, ValueSubtype.Base] += value.Base;
                    this[ValueType.ContactDamage, ValueSubtype.Modifier] += value.Modifier;
                    break;
                default:
                    Debug.LogWarning(gameObject.name + " - EntityAttributes::ApplyEffect(): Unknown value type: " + value.Type.ToString());
                    break;
            }

        }
        

        public void ApplyEffect(ValueType type, float value) {
            ApplyEffect(new ValueCollection.Value { Type = type, Base = value });
        }

        #endregion
    }
}