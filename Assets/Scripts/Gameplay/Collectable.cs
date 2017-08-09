using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay {
    [DisallowMultipleComponent]
    public class Collectable : MonoBehaviour {

        public float RotationSpeed = 30;

        public List<ValueCollection.Preset> Contents;

        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {
            gameObject.transform.Rotate(0, RotationSpeed * Time.deltaTime, 0, Space.World);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.transform.root.tag == "Player") {
                var stats = other.gameObject.transform.root.GetComponent<EntityStats>();

                foreach (var preset in Contents)
                    stats[preset.Type, preset.Subtype] += preset.Value;

                Contents.Clear(); // Handle exceptional cases of multiple-triggering

                Destroy(gameObject);
            }
        }
    }
}