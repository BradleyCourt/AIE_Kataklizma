using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntityStats))]
    public class Collectable : MonoBehaviour {

        public float RotationSpeed = 30;

        protected EntityStats Stats;

        // Use this for initialization
        void Start() {
            Stats = GetComponent<EntityStats>();
            if (Stats == null) throw new System.ApplicationException(gameObject.name + " - Collectable component could not locate required EntityStats sibling");
        }

        // Update is called once per frame
        void Update() {
            gameObject.transform.Rotate(0, RotationSpeed * Time.deltaTime, 0, Space.World);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.transform.root.tag == "Player") {
                other.gameObject.transform.root.GetComponent<EntityStats>()[ValueType.Experience, ValueSubtype.Base] += Stats[ValueType.Experience];
            }
        }
    }
}