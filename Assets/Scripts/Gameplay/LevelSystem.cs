using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace Kataklizma.Gameplay {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntityAttributes))]
    public class LevelSystem : MonoBehaviour {

        protected EntityAttributes Stats;
        public Pause p;

        [System.Serializable]
        public struct LevelUpOptions {

            public int XpThreshold;
            public List<ValueCollection.Value> Effects;
            //public List<Scriptables.ScriptedAbility> AddAbilities;
        }

        public List<LevelUpOptions> Levels;
        
        public int CurrentLevel {
            get { return (int)Stats[ValueType.CharacterLevel];  }
            protected set { Stats[ValueType.CharacterLevel, ValueSubtype.Base] = value; }
        }

        public int CurrentXp {
            get { return (int)Stats[ValueType.Experience]; }
            protected set { Stats[ValueType.Experience, ValueSubtype.Base] = value; }

        }

        public int PreviousXpThreshold {
            get {
                if (CurrentLevel == 1 || Levels.Count == 1) return 0;
                if (CurrentLevel >= Levels.Count) return Levels[Levels.Count - 2].XpThreshold;
                return Levels[CurrentLevel - 2].XpThreshold;
            }
        }

        public int CurrentXpThreshold {
            get {
                return Levels[Mathf.Min(CurrentLevel - 1, Levels.Count - 1)].XpThreshold;
            }
        }

        public bool HasMoreLevels { get { return CurrentLevel < Levels.Count; } }


        // Use this for initialization
        void Start() {

            Stats = GetComponent<EntityAttributes>();
            if (Stats == null) throw new System.ApplicationException(gameObject.name + " - LevelSystem: Could not locate required EntityStats sibling.");

            if (Levels.Count < 1) throw new System.ApplicationException(gameObject.name + " - LevelSystem: Levels cannot be empty.");
            

            Stats[ValueType.CharacterLevel, ValueSubtype.Base] = 1;
            //Stats[ValueType.ExperienceThreshold, ValueSubtype.Base] = CurrentXpThreshold;
            DoLevelGained();

            Stats.ValueChanged += OnPlayerStatsValueChanged;
        }
        
        void OnDestroy() {
            Stats.ValueChanged -= OnPlayerStatsValueChanged;
        }

        // Update is called once per frame
        void Update()
        {


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="type"></param>
        /// <param name="subtype"></param>
        /// <param name="old"></param>
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

        /// <summary>
        /// 
        /// </summary>
        private void DoExperienceGained() {
            var XP = Stats[ValueType.Experience];

            bool shouldLevelUp = HasMoreLevels && XP >= CurrentXpThreshold;

            if (shouldLevelUp) // if the characters level is no longer = to your current level
                CurrentLevel++; // add level
        }

        /// <summary>
        /// 
        /// </summary>
        private void DoLevelGained() {
            Stats[ValueType.ExperienceThreshold, ValueSubtype.Base] = CurrentXpThreshold;


            //transform.localScale = Vector3.one * Mathf.Pow(2, Stats[ValueType.CharacterLevel] - 1);
            LeanTween.value(gameObject, (value) =>
            {
                transform.localScale = value;
            }, transform.localScale, Vector3.one * Mathf.Pow(2, Stats[ValueType.CharacterLevel] - 1), 0.5f);

            Stats.ApplyEffects(Levels[CurrentLevel-1].Effects);
        }
    }
}