using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Kataklizma.Gameplay.Character {

    /// <summary>
    /// Inspired by Unity StandardAssets ThridPersonCharacter
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]

    public class CharacterManager : MonoBehaviour {


        public event System.Action<object> AbilityActivated;

        public bool IsGrounded {
            get {
                return Physics.Raycast(transform.position, -transform.up, 1.0001f);
            }
        }

        public bool IsCrouched { get; protected set; }
        public float IdleTime { get; protected set; }

        public bool IsInputLocked { get; protected set; }

        [Tooltip("If null, will search for first non-trigger capsule collider sibling.")]
        public CapsuleCollider Capsule;

        Animator Anim;
        Rigidbody Rb;

        protected List<ActivatedAbility> CoolingDown = new List<ActivatedAbility>();

        // Use this for initialization
        void Start() {

            Anim = GetComponent<Animator>();
            if (Anim == false) throw new System.ApplicationException(gameObject.name + " - CharacterManager: Failed to locate required Animator sibling.");

            Rb = GetComponent<Rigidbody>();
            if (Rb == false) throw new System.ApplicationException(gameObject.name + " - CharacterManager: Failed to locate required Animator sibling.");


            if (Capsule == null) Capsule = GetComponents<CapsuleCollider>().FirstOrDefault(m => !m.isTrigger);
            if (Capsule == null) throw new System.ApplicationException(gameObject.name + " - CharacterManager: Failed to locate required CapsuleCollide sibling.");

            
        }

        void Update() {
            foreach (var ability in CoolingDown) {
                Melee.UpdateCooldown(Time.deltaTime);
            }
        }

        /*  Character Actions:
         *      * Move
         *      * Dash
         *      * Leap
         *      * Melee Attack
         *      * Range Attack
         *      * Aura Attack
         */

        public void Move(Vector3 move, bool crouch) {

        }

        void HandleGroundMovement(bool crouch) {

        }



        void UpdateAnimator() {

        }

        #region " Ability: Melee "
        public string MeleeAnimation;

        public float MeleeChargeTime;

        [System.Serializable]
        public class ActivatedAbility {
            public enum Stage {
                Ready,
                Charging,
                Channelling,
                Cleanup,
                Cooldown,
            }

            public string ChargingAnimation;
            public string ChannelingAnimation;
            public float ChargeTime;
            public float ChannelTime;
            public float CleanupTime;
            public float CooldownTime;

            public Stage ActivationStage { get; set; }

            public float RemainingChargeTime { get; set; }
            public float RemainingChannelTime { get; set; }
            public float RemainingCleanupTime { get; set; }
            public float RemainingCooldownTime { get; set; }

            /// <summary>
            /// Returns boolean to show if charge cycle is complete (ChargeTime)
            /// </summary>
            public bool Charge(bool triggerState) {
                switch (ActivationStage) {
                    case Stage.Ready:
                        // Begin Charging
                        RemainingChargeTime = ChargeTime;
                        ActivationStage = Stage.Charging;
                        break;
                    case Stage.Charging:
                        RemainingChargeTime = Mathf.Max(RemainingChargeTime - Time.deltaTime, 0);
                        break;
                    default:
                        //throw new System.InvalidOperationException("CharacterManager::ActivatedAbility::Charge(): Invalid AnimationStage.");
                        return true; // Invalid state, force 'cycle ended'
                }

                return (RemainingChargeTime == 0);
            }

            /// <summary>
            /// Returns boolean to show if activation cycle is complete (ChannelTime)
            /// </summary>
            public bool Channel(bool triggerState) {
                switch (ActivationStage) {
                    case Stage.Charging:
                        // Start Cooldown
                        RemainingCooldownTime = CooldownTime;
                        RaiseCooldownStarted();

                        // Begin Discharging (Channeling)
                        RemainingChannelTime = ChargeTime;
                        ActivationStage = Stage.Channelling;
                        break;
                    case Stage.Channelling:
                        RemainingChannelTime = Mathf.Max(RemainingChargeTime - Time.deltaTime, 0);
                        break;
                    default:
                        //throw new System.InvalidOperationException("CharacterManager::ActivatedAbility::Channel(): Invalid AnimationStage.");
                        return true;  // Invalid state, force 'cycle ended'
                }

                return (RemainingChannelTime == 0);
            }

            /// <summary>
            /// Returns boolean to show if cleanup cycle is complete (CleanupTime)
            /// </summary>
            public bool End(bool triggerState) {
                switch (ActivationStage) {
                    case Stage.Ready: // Stage = "Ready"
                        // Irrelevant
                        break;
                    case Stage.Charging: // Stage = "Charging"
                        // End Charging
                        ActivationStage = Stage.Ready; // No Cleanup period required
                        RemainingCleanupTime = 0;
                        break;
                    case Stage.Channelling: // Stage = "
                        // End Activation
                        ActivationStage = Stage.Cleanup;
                        RemainingCleanupTime = CleanupTime;
                        break;
                    case Stage.Cleanup:
                        RemainingCleanupTime = Mathf.Max(RemainingCleanupTime - Time.deltaTime, 0);
                        if ( RemainingCleanupTime == 0 ) {
                            ActivationStage = (RemainingCooldownTime == 0) ? Stage.Ready : Stage.Cooldown;
                        }
                        break;
                    default:
                        //throw new System.InvalidOperationException("CharacterManager::ActivatedAbility::End(): Invalid AnimationStage.");
                        return true;  // Invalid state, force 'cycle ended'
                }

                return (RemainingCleanupTime == 0);
            }

            public event System.Action<object, bool> CooldownStateChanged;

            protected void RaiseCooldownStarted() {
                if (CooldownStateChanged != null)
                    CooldownStateChanged(this, (RemainingCooldownTime != 0));
            }

            public void UpdateCooldown( float deltaTime ) {
                RemainingCooldownTime = Mathf.Max(RemainingCooldownTime - deltaTime, 0);

            }
        }

        public ActivatedAbility Melee;


        #endregion
        #region " Ability: Dash "

        [Header("Dash")]
        public string DashAnimation;

        public float DashChargeTime = 3.0f;

        protected float DashChargeTimeRemaining;

        public int DashAnimStage { get; protected set; }




        #endregion

        private void OnCollisionEnter(Collision collision) {
            // Stop Dashing
        }
    }
}