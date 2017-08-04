using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


namespace Gameplay {

    [DisallowMultipleComponent]
    public class EntityStats : MonoBehaviour {

        [System.Serializable]
        public class StartingStat {
            public ValueType Name;
            public ValueSubtype _ValueType;
            public float _Value;
        }

        public List<StartingStat> StartingStats;

        #region " events "
        public event System.Action<Object, ValueType, ValueSubtype, float> ValueChanged;

        protected void RaiseValueChanged(ValueType type, ValueSubtype subtype, float oldValue) {
            if (ValueChanged != null) ValueChanged(this, type, subtype, oldValue);
        }
        #endregion

        #region " Stat collection "

        protected ValueCollection _Stats = new ValueCollection();

        public float this[ValueType type, ValueSubtype subtype = ValueSubtype.Derived] {
            get { return _Stats[type, subtype]; }
            set { _Stats[type, subtype] = value; }
        }

        #endregion

        public bool IsAlive { get { return this[ValueType.Health] > 0; } }


        // Use this for initialization
        void Start() {
            //
            // *** NOTE: UI should have its own scripts that hook into StatChanged events! ***
            //
            //if (HealthBar != null)
            //{
            //HealthBar.maxValue = MaxHealth;
            //HealthBar.minValue = 0;
            //}

            foreach (var stat in StartingStats) {
                this[stat.Name, stat._ValueType] = stat._Value;
            }

            _Stats.ValueChanged += OnStatsValueChanged;
        }

        private void OnStatsValueChanged(object arg1, ValueType arg2, ValueSubtype arg3, float arg4) {
            Debug.Log("Stat Value Changed: " + arg2.ToString() + ", " + arg3.ToString() + ", " + arg4.ToString("N2") + " -> " + _Stats[arg2, arg3].ToString("N2"));
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