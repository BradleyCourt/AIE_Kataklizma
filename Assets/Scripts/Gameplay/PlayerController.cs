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

            public bool CanActivate { get { return Ability != null && Ability.CanActivate; } }

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

        protected Rigidbody Rb;
        protected Animator CharacterAnimator;

        [System.Serializable]
        public struct ObserverOptions {
            public float Distance;
            public Vector3 Focus;

            [HideInInspector]
            public Camera Camera;

            [HideInInspector]
            public float Theta;
            [HideInInspector]
            public float Phi;
        }

        public ObserverOptions Observer;


        void Start() {
            Observer.Camera = GetComponentInChildren<Camera>();
            if (Observer.Camera == null) throw new System.ApplicationException(gameObject.name + " - PlayerController: Unable to locate required Camera child");

            Rb = GetComponent<Rigidbody>();
            if (Rb == null) throw new System.ApplicationException(gameObject.name + " - PlayerController requires a Rigidbody sibling");

            CharacterAnimator = GetComponentInChildren<Animator>();
            if (CharacterAnimator == null) Debug.LogWarning(gameObject.name + " - PlayerController::Start(): Unable to locate Animation component in child.");

            IsControllable = true;

            foreach (var slot in Abilities) {
                if (slot.Ability != null) {
                    slot.Ability.Bind(transform);
                    slot.Ability.Reset();
                }
            }
        }


        // Update is called once per frame
        void Update() {
            UpdateInputState();
                        

            if ( ActiveAbility != null ) {
                var continuing = ActiveAbility.Ability.OnUpdate(Input.GetButton(ActiveAbility.TriggerName));

                if (!continuing)
                    ActiveAbility = null;                
            }

            if ( ActiveAbility == null ) {
                foreach (var slot in Abilities) {
                    if ( slot.CanActivate && Input.GetButton(slot.TriggerName)) {
                        if (slot.Ability.OnBegin()) {
                            ActiveAbility = slot;
                            break;
                        }
                    }
                }
            }

            // Move character
            Vector3 motion = GetPlayerMotion() * MoveSpeed;
            transform.LookAt(transform.position + motion);

            UpdateCamera();

            var velocity = Rb.velocity;
            velocity.x = 0;
            velocity.z = 0;
            velocity += motion;

            

            if (velocity.magnitude > 0) {
                Rb.velocity = velocity;
            }

            CharacterAnimator.SetFloat("WalkSpeed", motion.magnitude / MoveSpeed);

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
                Observer.Theta = Mathf.Repeat(Observer.Theta + Input.GetAxis("ViewHorizontal"), 360);
                Observer.Phi = Mathf.Clamp(Observer.Phi + Input.GetAxis("ViewVertical"), 20, 70);
            }


            // Apply Camera
            var yaw = Quaternion.Euler(0, Observer.Theta, 0);
            var pitch = Quaternion.Euler(-Observer.Phi, 0, 0);

            var offset = Vector3.forward * Observer.Distance;

            offset = pitch * offset;
            offset = yaw * offset;

            // Rotate camera around target
            Observer.Camera.transform.position = (transform.position + offset);

            // Point camera at target
            Observer.Camera.transform.LookAt(transform.TransformPoint(Observer.Focus));

        }

        /// <summary>
        /// Apply captured motion to character.  Update character velocity.
        /// </summary>
        Vector3 GetPlayerMotion() {

            if (!IsControllable) return Vector3.zero; // Player-control disabled

            var cameraDirection = transform.position - Observer.Camera.transform.position;

            var characterFwd = new Vector3(cameraDirection.x, 0, cameraDirection.z).normalized;
            var characterRight = new Vector3(cameraDirection.z, 0, -cameraDirection.x).normalized;

            var motion = characterFwd * Input.GetAxis("MoveVertical") + characterRight * Input.GetAxis("MoveHorizontal");
            motion.Normalize();
            
            return motion;
        }


        void OnTriggerEnter(Collider col) {


        }


        private void OnCollisionEnter(Collision collision) {

        }
    }
}