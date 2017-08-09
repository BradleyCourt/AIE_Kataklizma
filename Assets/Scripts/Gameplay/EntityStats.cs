using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


namespace Gameplay {

    [DisallowMultipleComponent]
    public class EntityStats : MonoBehaviour {


        #region " Stats "
        public List<ValueCollection.Preset> StartingStats;

        public event System.Action<Object, ValueType, ValueSubtype, float> ValueChanged;

        protected void RaiseValueChanged(ValueType type, ValueSubtype subtype, float oldValue) {
            if (ValueChanged != null) ValueChanged(this, type, subtype, oldValue);
        }


        protected ValueCollection _Stats;

        public float this[ValueType type, ValueSubtype subtype = ValueSubtype.Derived] {
            get { return _Stats[type, subtype]; }
            set { _Stats[type, subtype] = value; }
        }

        #endregion

        public bool IsAlive { get { return this[ValueType.Health] > 0; } }


        public EntityStats() {
            _Stats = new ValueCollection();
            _Stats.ValueChanged += OnStatsValueChanged;

        }

        // Use this for initialization
        void Start() {
            
            _Stats.Parse(StartingStats);
        }

        private void OnStatsValueChanged(object arg1, ValueType arg2, ValueSubtype arg3, float arg4) {
            Debug.Log("Stat Value Changed: " + arg2.ToString() + ", " + arg3.ToString() + ", " + arg4.ToString("N2") + " -> " + _Stats[arg2, arg3].ToString("N2"));
            RaiseValueChanged(arg2, arg3, arg4);
        }



        // Update is called once per frame
        void Update() {

        }

        public void RemoveHealth(int Damage) {
            this[ValueType.Health, ValueSubtype.Base] -= Damage;

            // NOTE: "Object Death" no longer needs a broadcast.  Attach an event listener instead.
        }
    }
}