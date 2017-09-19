using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scriptables;

namespace Gameplay {
    
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour {

        [System.Serializable]
        public class AbilitySlot {
            public string TriggerName;
            public ActivatedAbility Ability;
        }

        private bool IsGrounded {
            get {
                return Physics.Raycast(transform.position, -transform.up, 1.0001f);
            }
        }

        private float _IdleTime;
        public float IdleTime {
            get { return _IdleTime; }
            protected set { _IdleTime = value; }
        }

        public float MoveSpeed = 5.0f;


        public bool IsControllable { get; set; }


        public GameObject CuboidZonePrefab;

        
        protected AbilitySlot ActiveAbility = null;
        public List<AbilitySlot> Abilities;

        private Rigidbody Rb;
        private Camera ObserverCam;
        private float ObserverTheta;
        private float ObserverPhi;

        [Header("Observer Camera")]
        public float ObserverDistance;
        public float ObserverOffset;
        
        //#region " Action: Jump "

        //public float JumpVel = 8.0F;
        //public float JumpCooldown = 0;
        //public float JumpDuration = 0.2f;
        //private bool IsJumpReady = true;
        //private bool CanJump {
        //    get {
        //        return IsJumpReady && IsGrounded && State == AnimState.Idle;
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //void DoJump() {
        //    State = AnimState.Jumping;
        //    IsJumpReady = false;

        //    // Setup delay for returning control "when it stops"
        //    StartCoroutine(this.DelayedAction(JumpDuration,
        //        () => {
        //            if (State == AnimState.Jumping) {
        //                State = AnimState.Idle;
        //            }
        //        }));

        //    // Setup delay for cooldown "when it can be done again"
        //    StartCoroutine(this.DelayedAction(JumpCooldown,
        //        () => {
        //            IsJumpReady = true;
        //        }));

        //    Rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
        //}
        //#endregion



        // Use this for initialization


        void Start() {
            ObserverCam = GetComponentInChildren<Camera>();
            if (ObserverCam == null) throw new System.ApplicationException(gameObject.name + " - PlayerController: Unable to locate required Camera child");

            Rb = GetComponent<Rigidbody>();
            if (Rb == null) throw new System.ApplicationException("PlayerController requires a Rigidbody sibling");

            IsControllable = true;
        }


        // Update is called once per frame
        void Update() {
            UpdateInputState();

            //Debug.Log("Trigger State (Fire1) : " + Input.GetButton("Fire1").ToString());
            var showLog = false;
            var sb = new System.Text.StringBuilder(gameObject.name + " - PlayerController::Update() : ");
            
            if ( ActiveAbility != null ) {
                sb.Append("Active[" + ActiveAbility.TriggerName + "|" + Input.GetButton(ActiveAbility.TriggerName) + "]");
                showLog = true;

                var continuing = false;
                if ( ActiveAbility.Ability.ActivationState != AbilityActivationState.Cleanup)
                    continuing = ActiveAbility.Ability.OnUpdate(Input.GetButton(ActiveAbility.TriggerName));

                sb.Append(", cont[" + continuing + "]");

                var ended = false;
                if (!continuing) {
                    ended = ActiveAbility.Ability.OnEnd();
                    sb.Append(", ended[" + ended + "]");
                }

                if (ended)
                    ActiveAbility = null;
                
            }

            if ( ActiveAbility == null ) {
                foreach (var slot in Abilities) {
                    if ( slot.Ability.CanActivate && Input.GetButton(slot.TriggerName)) {
                        sb.Append("Triggered[" + slot.TriggerName + "]");
                        showLog = true;
                        if (slot.Ability.OnBegin()) {
                            sb.Append(", Began");
                            ActiveAbility = slot;
                        }
                    }
                }
            }

            Debug.Log(sb.ToString());

            // Move character
            Vector3 motion = GetPlayerMotion() * MoveSpeed;
            transform.LookAt(transform.position + motion);

            UpdateCamera();

            var velocity = Rb.velocity;
            velocity.x = 0;
            velocity.z = 0;
            velocity += motion; 

            if (velocity.magnitude > 0)
                Rb.velocity = velocity;
            Rb.angularVelocity = Vector3.zero;

        }

        void UpdateInputState() {
            if (Input.GetMouseButtonDown(2)) { // Middle Mouse toggles mouse capture
                if (Cursor.lockState != CursorLockMode.Locked) {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                } else {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }

        void UpdateCamera() {
            if (Cursor.lockState == CursorLockMode.Locked) {

                // Update Camera
                ObserverTheta = Mathf.Repeat(ObserverTheta + Input.GetAxis("ViewHorizontal"), 360);
                ObserverPhi = Mathf.Clamp(ObserverPhi + Input.GetAxis("ViewVertical"), 20, 70);
            }


            // Apply Camera
            var yaw = Quaternion.Euler(0, ObserverTheta, 0);
            var pitch = Quaternion.Euler(-ObserverPhi, 0, 0);

            var offset = Vector3.forward * ObserverDistance;

            offset = pitch * offset;
            offset = yaw * offset;

            // Rotate camera around target
            ObserverCam.transform.position = (transform.position + offset);

            // Point camera at target
            ObserverCam.transform.LookAt(transform);
            ObserverCam.transform.Rotate(ObserverOffset, 0, 0, Space.Self);

        }

        /// <summary>
        /// Apply captured motion to character.  Update character velocity.
        /// </summary>
        Vector3 GetPlayerMotion() {

            if (!IsControllable) return Vector3.zero; // Player-control disabled

            var cameraDirection = transform.position - ObserverCam.transform.position;

            var characterFwd = new Vector3(cameraDirection.x, 0, cameraDirection.z).normalized;
            var characterRight = new Vector3(cameraDirection.z, 0, -cameraDirection.x).normalized;

            var motion = characterFwd * Input.GetAxis("MoveVertical") + characterRight * Input.GetAxis("MoveHorizontal");
            motion.Normalize();
            
            return motion;
        }


        void OnTriggerEnter(Collider col) {

            //if (State == AnimState.Charging) {
            //    if (col.gameObject.tag == "Enemy") {
            //        State = AnimState.Idle;
            //    }
            //}
        }


        private void OnCollisionEnter(Collision collision) {
            //if (State == AnimState.Charging) {
            //    State = AnimState.Idle;
            //    IsControllable = true;
            //}
        }

        //    void OnDrawGizmos() {

        //        Gizmos.color = new Color(1, 0, 0, 0.5f);

        //        if (State == AnimState.Attacking) {
        //            Gizmos.matrix = gameObject.transform.localToWorldMatrix;

        //            if (!IsMeleeAttackReady) {
        //                var centre = MeleeAttackZone.center;
        //                var size = MeleeAttackZone.size;

        //                Gizmos.DrawCube(centre, size);
        //            }
        //            if (!IsRangedAttackReady) {
        //                var centre = RangedAttackZone.center;
        //                var size = RangedAttackZone.size;

        //                Gizmos.DrawCube(centre, size);
        //            }
        //        }

        //    }
    }
}