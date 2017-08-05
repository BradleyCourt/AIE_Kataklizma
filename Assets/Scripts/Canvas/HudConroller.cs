using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;

namespace Canvas {
    public class HudConroller : MonoBehaviour {

        [System.Serializable]
        public struct DataSources {
            public EntityStats PlayerStats;
            public LevelSystem LevelSystem;
        }

        [System.Serializable]
        public struct UiWidgets {
            public UnityEngine.UI.Slider XpBar;
        }

        public DataSources Sources;
        public UiWidgets Widgets;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}