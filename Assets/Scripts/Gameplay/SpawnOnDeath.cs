//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kataklizma.Gameplay {

    [RequireComponent(typeof(EntityAttributes))]
    public class SpawnOnDeath : MonoBehaviour {

        public Vector3 OriginOffset = new Vector3(2.5f, 0, 2.5f);
        
        public Transform OptionalParent;
        
        [UnityEngine.Serialization.FormerlySerializedAs("CollectableRadius")]
        public float SpawnRadius = 2;

        [Space]
        public List<GameObject> MustSpawn;

        [Space]
        public int LotterySpawnCount;
        public List<GameObject> LotterySpawn;
        public GameObject RandomLotteryItem {  get { return LotterySpawn[Random.Range(0, LotterySpawn.Count)]; } }


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

                foreach ( var prefab in MustSpawn)
                {
                    if (prefab == null) continue;

                    var offset2D = Random.insideUnitCircle * SpawnRadius;
                    var offset = OriginOffset + new Vector3(offset2D.x, 0, offset2D.y);

                    Instantiate(prefab, gameObject.transform.position + offset, gameObject.transform.rotation, OptionalParent);
                }


                if (LotterySpawn.Count > 0)
                {
                    for (int i = 0; i < LotterySpawnCount; i++)
                    {
                        var prefab = RandomLotteryItem;

                        if (prefab == null) continue;

                        var offset2D = Random.insideUnitCircle * SpawnRadius;
                        var offset = OriginOffset + new Vector3(offset2D.x, 0, offset2D.y);

                        Instantiate(prefab, gameObject.transform.position + offset, gameObject.transform.rotation, OptionalParent);
                    }
                }                
            }
        }
    }
}