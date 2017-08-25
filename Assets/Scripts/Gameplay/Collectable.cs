using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay {
    [DisallowMultipleComponent]
    public class Collectable : MonoBehaviour {

        public float RotationSpeed = 30;
        
        [UnityTag]
        public List<string> CollectableBy;

        public List<ValueCollection.Value> Contents;

        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {
            gameObject.transform.Rotate(0, RotationSpeed * Time.deltaTime, 0, Space.World);
        }

        private void OnTriggerEnter(Collider other) {
            if (CollectableBy.Contains(other.gameObject.transform.root.tag)) {
                var stats = other.gameObject.transform.root.GetComponent<EntityAttributes>();

                foreach (var preset in Contents) {
                    stats[preset.Type, ValueSubtype.Base] += preset.Base;
                    stats[preset.Type, ValueSubtype.Modifier] += preset.Modifier;
                }

                // Handle exceptional cases of multiple-triggering
                Contents.Clear(); 

                Destroy(gameObject);
            }
        }
    }
}