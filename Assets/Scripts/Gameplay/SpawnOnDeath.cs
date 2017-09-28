//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay {

    [RequireComponent(typeof(EntityAttributes))]
    public class SpawnOnDeath : MonoBehaviour {
        
        public float CollectableRadius = 2;
        public GameObject CollectablePrefab;
        
        private EntityAttributes Stats;
        private bool HasSpawned = false;

        // Use this for initialization
        void Start() {

            Stats = GetComponent<EntityAttributes>();
            if (Stats == null) throw new System.ApplicationException(gameObject.name + " - CollapseOnDeath: Could not locate required EntityStats sibling.");

            Stats.ValueChanged += OnStatsValueChanged;

        }

        private void OnStatsValueChanged(UnityEngine.Object sender, ValueType type, ValueSubtype subtype, float old) {

            if ( type == ValueType.Health && Stats[ValueType.Health] <= 0 && !HasSpawned ) {
                HasSpawned = true;

                if (CollectablePrefab == null) {
                    Debug.LogWarning(gameObject.name + " - SpawnOnDeath: CollectablePrefab is null");
                    return;
                }

                // Spawn Collectables

                // foreach (prefab)
                var offset = Random.insideUnitCircle * CollectableRadius;
                Instantiate(CollectablePrefab, gameObject.transform.position + new Vector3(offset.x, 0, offset.y), new Quaternion());
            }
        }
    }
}