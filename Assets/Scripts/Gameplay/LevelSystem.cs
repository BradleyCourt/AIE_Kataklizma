using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntityStats))]
    public class LevelSystem : MonoBehaviour {

        protected EntityStats Stats;

        public List<int> XpThresholds;

        protected int CurrentLevel {
            get { return (int)Stats[ValueType.CharacterLevel];  }
            set { Stats[ValueType.CharacterLevel, ValueSubtype.Base] = value; }
        }

        protected int CurrentXp {
            get { return (int)Stats[ValueType.Experience]; }
            set { Stats[ValueType.Experience, ValueSubtype.Base] = value; }
        }

        public int PreviousXpThreshold {
            get {
                if (CurrentLevel == 1 || XpThresholds.Count == 1) return 0;
                if (CurrentLevel >= XpThresholds.Count) return XpThresholds[XpThresholds.Count - 2];
                return XpThresholds[CurrentLevel - 2];
            }
        }

        public int CurrentXpThreshold {
            get {
                return XpThresholds[Mathf.Min(CurrentLevel - 1, XpThresholds.Count - 1)];
            }
        }

        public bool HasMoreLevels { get { return CurrentLevel < XpThresholds.Count; } }


        // Use this for initialization
        void Start() {

            Stats = GetComponent<EntityStats>();
            if (Stats == null) throw new System.ApplicationException(gameObject.name + " - LevelSystem: Could not locate required EntityStats sibling.");

            if (XpThresholds.Count < 1) throw new System.ApplicationException(gameObject.name + " - LevelSystem: XpThresholds cannot be empty.");
            
            Stats.ValueChanged += OnPlayerStatsValueChanged;

            CurrentLevel = 1;
        }
        
        // Update is called once per frame
        void Update() {

        }


        private void OnPlayerStatsValueChanged(Object sender, ValueType type, ValueSubtype subtype, float old) {
            
            switch ( type ) {
                case ValueType.Experience:
                    // Ensure CharacterLevel is correct for current accrued Experience
                    DoExperienceGained();
                    break;
                case ValueType.CharacterLevel:
                    // Ensure ExperienceThreshold is correct for new current level
                    DoLevelGained();
                    break;
            }
        }

        private void DoExperienceGained() {
            var XP = Stats[ValueType.Experience];

            bool shouldLevelUp = HasMoreLevels && XP >= CurrentXpThreshold;

            if (shouldLevelUp) // if the characters level is no longer = to your current level
                CurrentLevel++; // add level
        }

        private void DoLevelGained() {
            Stats[ValueType.ExperienceThreshold] = CurrentXpThreshold;
        }
    }
}