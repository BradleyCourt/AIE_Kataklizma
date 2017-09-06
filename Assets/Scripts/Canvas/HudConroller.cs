using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;

namespace Kataklizma.Canvas {
    public class HudConroller : MonoBehaviour {

        [System.Serializable]
        public struct DataSources {
            public EntityAttributes PlayerStats;

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
            if (Widgets.XpBar != null) {
                Widgets.XpBar.minValue = 0;
                Widgets.XpBar.value = Sources.PlayerStats[ValueType.Experience];
                Widgets.XpBar.maxValue = Sources.PlayerStats[ValueType.ExperienceThreshold];
            }

            if (Widgets.HealthBar != null) {
                Widgets.HealthBar.minValue = 0;
                Widgets.HealthBar.value = Sources.PlayerStats[ValueType.Health];
                Widgets.HealthBar.maxValue = Sources.PlayerStats[ValueType.HealthMax];
            }

            Sources.PlayerStats.ValueChanged += OnPlayerStatsValueChanged;
        }

        private void OnPlayerStatsValueChanged(Object source, ValueType type, ValueSubtype subtype, float old) {
            
            switch (type) {
                case ValueType.Experience:
                    // Update XP Bar Value
                    if (Widgets.XpBar != null) {
                        Widgets.XpBar.value = (source as EntityAttributes)[type];
                    }
                    break;
                case ValueType.ExperienceThreshold:
                    // Update XP Bar Extents (Min and Max)
                    if (Widgets.XpBar != null) {
                        Widgets.XpBar.minValue = old;
                        Widgets.XpBar.maxValue = (source as EntityAttributes)[type];
                    }
                    break;
                case ValueType.Health:
                    if ( Widgets.HealthBar != null ) {
                        Widgets.HealthBar.value = (source as EntityAttributes)[type];
                    }
                    break;
                case ValueType.HealthMax:
                    if ( Widgets.HealthBar != null )
                    {
                        Widgets.HealthBar.maxValue = (source as EntityAttributes)[type];
                        Widgets.HealthBar.value = (source as EntityAttributes)[ValueType.Health];
                    }
                    break;
            }
        }

        // Update is called once per frame
        void Update() {

        }
    }
}