using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay {
    public class CollapseOnDeath : MonoBehaviour {
        private bool IsFalling { get; set; }


        public bool UseSceneGravity = true;
        public float LocalGravity = 0.05f; // "Fall Speed"
        public float MaxFallSpeed = 1.0f;

        public float TimeToLive = 1; // "Fall Time"

        private Rigidbody Rb;
        private Vector3 _Velocity;


        /// Tracks velocity on a rigidbody if it exists, else tracks it locally
        private Vector3 Velocity {
            get {
                return Rb != null ? Rb.velocity : _Velocity;
            }
            set {
                if (Rb != null)
                    Rb.velocity = value;
                else
                    _Velocity = value;
            }
        }


        // Use this for initialization
        void Start() {
            IsFalling = false;

            Rb = GetComponent<Rigidbody>();
        }


        // Update is called once per frame
        void Update() {
            if (IsFalling == true) {
                Velocity += UseSceneGravity ? Physics.gravity : new Vector3(0, -LocalGravity, 0); // Increase velocity from gravity

                Velocity = Vector3.ClampMagnitude(Velocity, MaxFallSpeed); // Limit to MaxfallSpeed

                if (Rb == null) // If we have a rigidbody then volicty is already updating position.
                    transform.position += Velocity;
            }
        }


        void OnObjectDeath() {
            IsFalling = true;
            Destroy(gameObject, TimeToLive);
        }

    }
}