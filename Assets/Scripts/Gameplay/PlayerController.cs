using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay {

    [RequireComponent(typeof(ObserverDirector))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour {

        [System.Serializable]
        public struct Action {
            public float Cooldown;
            public float Duration;
            [HideInInspector]
            public bool IsReady;

            public System.Func<bool> CanPerform;
        }

        [System.Serializable]
        enum AnimState {
            Idle,
            Moving,
            Jumping,
            Attacking,
            Charging,
        }

        private bool IsGrounded {
            get {
                return Physics.Raycast(transform.position, -transform.up, 1.0001f);
            }
        }

        private AnimState State = AnimState.Idle;

        public float MoveSpeed = 5.0f;
        private bool CanMove {
            get {
                return State == AnimState.Idle;
            }
        }

        public bool IsControllable { get; set; }


        private ObserverDirector Observers;
        private Rigidbody Rb;

        #region " Action: Jump "

        public float JumpVel = 8.0F;
        public float JumpCooldown = 0;
        public float JumpDuration = 0.2f;
        private bool IsJumpReady = true;
        private bool CanJump {
            get {
                return IsJumpReady && IsGrounded && State == AnimState.Idle;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void DoJump() {
            State = AnimState.Jumping;
            IsJumpReady = false;

            // Setup delay for returning control "when it stops"
            StartCoroutine(this.DelayedAction(JumpDuration,
                () => {
                    if (State == AnimState.Jumping) {
                        State = AnimState.Idle;
                    }
                }));

            // Setup delay for cooldown "when it can be done again"
            StartCoroutine(this.DelayedAction(JumpCooldown,
                () => {
                    IsJumpReady = true;
                }));

            Rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
        #endregion

        #region " Action: Charge "

        public float ChargeSpeed = 7.5f;
        public float ChargeCooldown = 5; // Time before next charge is available
        public float ChargeDuration = 1.8f; // Time charge lasts for if not interrupted
        private bool IsChargeReady = true;
        private bool CanCharge {
            get {
                return IsChargeReady && IsGrounded && (State == AnimState.Idle || State == AnimState.Moving);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void DoCharge() {
            IsControllable = false;
            IsChargeReady = false;
            State = AnimState.Charging;

            // Setup delay for returning control
            StartCoroutine(this.DelayedAction(ChargeDuration,
                () => {
                    if (State == AnimState.Charging) { // If we're still 'charging' (ie, if we haven't hit anything already)
                    State = AnimState.Idle;
                        IsControllable = true;
                    }
                }));

            // Setup delay for cooldown
            StartCoroutine(this.DelayedAction(ChargeCooldown,
                () => {
                    IsChargeReady = true;
                }));

            // Do Charge
        }

        #endregion

        #region " Action: Melee Attack "

        public BoxCollider MeleeAttackZone;
        public float MeleeAttackCooldown = 1;
        public float MeleeAttackDuration = 0.3f;
        public int MeleeAttackDamage = 10;
        private bool IsMeleeAttackReady = true;
        private bool CanMeleeAttack {
            get {
                return IsMeleeAttackReady && State == AnimState.Idle;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void DoMeleeAttack() {
            Debug.Log("Do Melee Attack!");

            State = AnimState.Attacking;
            IsMeleeAttackReady = false;

            // Setup delay for returning control "when it stops"
            StartCoroutine(this.DelayedAction(MeleeAttackDuration,
                () => {
                    if (State == AnimState.Attacking) {
                        State = AnimState.Idle;
                    }
                }));

            // Setup delay for cooldown "when it can be done again"
            StartCoroutine(this.DelayedAction(MeleeAttackCooldown,
                () => {
                    IsMeleeAttackReady = true;
                }));



            // Do Attack
            var centreOffset = transform.localToWorldMatrix.MultiplyVector(MeleeAttackZone.center);

            var centre = transform.position + centreOffset;
            var size = MeleeAttackZone.size; // transform.localToWorldMatrix.MultiplyVector(MeleeAttackZone.size);

            Collider[] targets = Physics.OverlapBox(centre, size, transform.rotation, ~8); // Anything that is NOT a PlayerAvatar layer

            foreach (Collider target in targets) {
                if (target.gameObject.tag == "Player") continue;


                if (target.gameObject.tag == "Enemy" || target.gameObject.tag == "Building") {
                    var playerStats = target.GetComponent<EntityStats>();
                    if (playerStats) {
                        playerStats.RemoveHealth(MeleeAttackDamage);
                    }
                }
            }
        }
        #endregion

        #region " Action: Ranged Attack "

        public BoxCollider RangedAttackZone; // fire breath attack
        public float RangedAttackCooldown = 1;
        public float RangedAttackDuration = 0.3f;
        public int RangedAttackDamage = 15;
        private bool IsRangedAttackReady = true;
        private bool CanRangedAttack {
            get {
                return IsRangedAttackReady && State == AnimState.Idle;
            }
        }

        void DoRangedAttack() {
            Debug.Log("Do Ranged Attack!");

            State = AnimState.Attacking;
            IsRangedAttackReady = false;

            // Setup delay for returning control "when it stops"
            StartCoroutine(this.DelayedAction(RangedAttackDuration,
                () => {
                    if (State == AnimState.Attacking) {
                        State = AnimState.Idle;
                    }
                }));

            // Setup delay for cooldown "when it can be done again"
            StartCoroutine(this.DelayedAction(RangedAttackCooldown,
                () => {
                    IsRangedAttackReady = true;
                }));


            // Do Attack
            var centre = transform.position + transform.localToWorldMatrix.MultiplyVector(RangedAttackZone.center); ;
            var size = RangedAttackZone.size;

            Collider[] targets = Physics.OverlapBox(centre, size, transform.rotation, ~8); // Anything that is NOT a PlayerAvatar layer

            foreach (Collider target in targets) {
                if (target.gameObject.tag == "Player") continue;


                if (target.gameObject.tag == "Enemy" || target.gameObject.tag == "Building") {
                    var playerStats = target.GetComponent<EntityStats>();
                    if (playerStats) {
                        playerStats.RemoveHealth(RangedAttackDamage);
                    }
                }
            }

        }

        #endregion


        // Use this for initialization
        void Start() {
            Observers = GetComponent<ObserverDirector>();
            if (Observers == null) throw new System.ApplicationException("PlayerController requires an ObserverDirector sibling");

            Rb = GetComponent<Rigidbody>();
            if (Rb == null) throw new System.ApplicationException("PlayerController requires a Rigidbody sibling");

            IsControllable = true;
        }


        // Update is called once per frame
        void Update() {

            // Process input
            if (IsControllable) {
                if (CanCharge && Input.GetButtonDown("Charge"))
                    DoCharge();

                if (CanMeleeAttack && Input.GetButtonDown("Fire1"))
                    DoMeleeAttack();

                if (CanRangedAttack && Input.GetButtonDown("Fire2"))
                    DoRangedAttack();

                if (CanJump && Input.GetButtonDown("Jump"))
                    DoJump();


            }

            // Move character
            Vector3 motion;
            if (State != AnimState.Charging) {
                motion = GetPlayerMotion() * ChargeSpeed;
                Observers.Target.transform.LookAt(Observers.Target.transform.position + motion);
            } else {
                motion = transform.forward * ChargeSpeed;
            }


            var velocity = Rb.velocity;
            velocity.x = 0;
            velocity.z = 0;
            velocity += motion;

            if (velocity.magnitude > 0)
                Rb.velocity = velocity;
            Rb.angularVelocity = Vector3.zero;

        }


        /// <summary>
        /// Apply captured motion to character.  Update character velocity.
        /// </summary>
        Vector3 GetPlayerMotion() {

            if (!IsControllable) return Vector3.zero; // Player-control disabled

            var cameraDirection = Observers.Target.transform.position - Observers.Selected.Observer.transform.position;

            var characterFwd = new Vector3(cameraDirection.x, 0, cameraDirection.z).normalized;
            var characterRight = new Vector3(cameraDirection.z, 0, -cameraDirection.x).normalized;

            var motion = characterFwd * Input.GetAxis("MoveVertical") + characterRight * Input.GetAxis("MoveHorizontal");
            motion.Normalize();



            return motion;
        }


        void OnTriggerEnter(Collider col) {

            if (State == AnimState.Charging) {
                if (col.gameObject.tag == "Enemy") {
                    State = AnimState.Idle;
                }
            }
        }


        private void OnCollisionEnter(Collision collision) {
            if (State == AnimState.Charging) {
                State = AnimState.Idle;
                IsControllable = true;
            }
        }

        void OnDrawGizmos() {

            Gizmos.color = new Color(1, 0, 0, 0.5f);

            if (State == AnimState.Attacking) {
                Gizmos.matrix = gameObject.transform.localToWorldMatrix;

                if (!IsMeleeAttackReady) {
                    var centre = MeleeAttackZone.center;
                    var size = MeleeAttackZone.size;
 
                    Gizmos.DrawCube(centre, size);
                }
                if (!IsRangedAttackReady) {
                    var centre = RangedAttackZone.center;
                    var size = RangedAttackZone.size;

                    Gizmos.DrawCube(centre, size);
                }
            }

        }
    }
}