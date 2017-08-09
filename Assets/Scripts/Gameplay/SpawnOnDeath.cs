//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay {

    [RequireComponent(typeof(EntityStats))]
    public class SpawnOnDeath : MonoBehaviour {

        public GameObject RubblePrefab;


        public float CollectableRadius = 2;
        public GameObject CollectablePrefab;
        
        private EntityStats Stats;
        private bool HasSpawned = false;
        // Use this for initialization
        void Start() {

            Stats = GetComponent<EntityStats>();
            if (Stats == null) throw new System.ApplicationException(gameObject.name + " - CollapseOnDeath: Could not locate required EntityStats sibling.");

            Stats.ValueChanged += OnStatsValueChanged;

            if (RubblePrefab == null) Debug.LogWarning(gameObject + " - SpawnOnDeath: RubblePrefab is empty, this may cause holes in world");
        }

        private void OnStatsValueChanged(UnityEngine.Object sender, ValueType type, ValueSubtype subtype, float old) {
            if ( type == ValueType.Health && Stats[ValueType.Health] <= 0 && !HasSpawned ) {
                HasSpawned = true;


                // Spawn Rubble tile
                Instantiate(RubblePrefab, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);

                // Spawn Collectables

                // foreach (prefab)
                var offset = Random.insideUnitCircle * CollectableRadius;
                Instantiate(CollectablePrefab, gameObject.transform.position + new Vector3(offset.x, 0, offset.y), new Quaternion());
            }
        }
    }
}