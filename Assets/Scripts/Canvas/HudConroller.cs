using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;

namespace Canvas {
    public class HudConroller : MonoBehaviour {

        [System.Serializable]
        public struct DataSources {
            public EntityStats PlayerStats;

        }

        [System.Serializable]
        public struct UiWidgets {
            public UnityEngine.UI.Slider XpBar;
            public UnityEngine.UI.Slider HealthBar;
        }

        public DataSources Sources;
        public UiWidgets Widgets;

        // Use this for initialization
        void Start() {
            Widgets.XpBar.minValue = 0;
            Widgets.XpBar.value = Sources.PlayerStats[ValueType.Experience];
            Widgets.XpBar.maxValue = Sources.PlayerStats[ValueType.ExperienceThreshold];

            Widgets.HealthBar.minValue = 0;
            Widgets.HealthBar.value = Sources.PlayerStats[ValueType.Health];
            Widgets.HealthBar.maxValue = Sources.PlayerStats[ValueType.HealthMax];

            Sources.PlayerStats.ValueChanged += OnPlayerStatsValueChanged;
        }

        private void OnPlayerStatsValueChanged(Object source, ValueType type, ValueSubtype subtype, float old) {
            
            switch (type) {
                case ValueType.Experience:
                    // Update XP Bar Value
                    if (Widgets.XpBar != null) {
                        Widgets.XpBar.value = (source as EntityStats)[type];
                    }
                    break;
                case ValueType.ExperienceThreshold:
                    // Update XP Bar Extents (Min and Max)
                    if (Widgets.XpBar != null) {
                        Widgets.XpBar.minValue = old;
                        Widgets.XpBar.maxValue = (source as EntityStats)[type];
                    }
                    break;
                case ValueType.Health:
                    if ( Widgets.HealthBar != null ) {
                        Widgets.HealthBar.value = (source as EntityStats)[type];
                    }
                    break;
            }
        }

        // Update is called once per frame
        void Update() {

        }
    }
}