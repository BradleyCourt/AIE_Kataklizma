using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Kataklizma.Gameplay {

    /// <summary>
    /// Inspired by Unity StandardAssets ThridPersonCharacter
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class CharacterManager : MonoBehaviour {

        public bool IsGrounded { get; protected set; }
        public float IdleTime { get; protected set; }


        Animator Anim;
        Rigidbody Rb;

        // Use this for initialization
        void Start() {

            Anim = GetComponent<Animator>();
            if (Anim == false) throw new System.ApplicationException(gameObject.name + " - CharacterManager: Failed to locate required Animator sibling.");

            Rb = GetComponent<Rigidbody>();
            if (Rb == false ) throw new System.ApplicationException(gameObject.name + " - CharacterManager: Failed to locate required Animator sibling.");

        }


        public void Move(Vector3 move, bool crouch, bool jump) {

        }


        void HandleGroundMovement(bool crouch, bool jump) {

        }


        void HandleAirborneMovement() {
        }


        void UpdateAnimator() {

        }
    }
}