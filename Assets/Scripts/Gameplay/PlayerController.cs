using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scriptables;

namespace Gameplay {
    
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(EntityAttributes))]
    public class PlayerController : MonoBehaviour {

        [System.Serializable]
        public class AbilitySlot {
            public string TriggerName;
            public ScriptedAbility Ability;

            public bool CanActivate { get { return Ability != null && Ability.CanActivate; } }

        }

        private bool IsGrounded {
            get {
                return Physics.Raycast(transform.position, -transform.up, 1.0001f);
            }
        }

        private float LastActive;
        public float IdleTime {
            get { return Time.time - LastActive; }
        }

        //public float MoveSpeed = 5.0f;

        public bool IsControllable { get; set; }
        
        protected AbilitySlot UserActivatedAbility = null;
        protected List<ScriptedAbility> SystemActiveAbilities = new List<ScriptedAbility>();

        public List<AbilitySlot> Abilities;
        

        protected Rigidbody Rb;
        protected Animator CharacterAnimator;
        protected EntityAttributes Attributes;

        [System.Serializable]
        public struct ObserverOptions {
            [Tooltip("Min (x) and Max (y) Distance")]
            public Vector2 Range;
            [Tooltip("Min (x) and Max (y) Elevation")]
            public Vector2 Elevation;

            public Vector3 Focus;

            [HideInInspector]
            public Camera Camera;

            [HideInInspector]
            public float Theta;
            [HideInInspector]
            public float Phi;
            [HideInInspector]
            public float Distance;

        }

        public ObserverOptions Observer;


        /// <summary>
        /// 
        /// </summary>
        void Start() {
            Observer.Camera = GetComponentInChildren<Camera>();
            if (Observer.Camera == null) Debug.LogError(gameObject.name + " - PlayerController: Unable to locate required Camera child");

            Rb = GetComponent<Rigidbody>();
            if (Rb == null) Debug.LogError(gameObject.name + " - PlayerController::Start(): Unable to locate required Rigidbody sibling");

            CharacterAnimator = GetComponentInChildren<Animator>();
            if (CharacterAnimator == null) Debug.LogWarning(gameObject.name + " - PlayerController::Start(): Unable to locate Animation component in child");

            Attributes = GetComponent<EntityAttributes>();
            if (CharacterAnimator == null) Debug.LogError(gameObject.name + " - PlayerController::Start(): Unable to locate required EntityAttributes sibling");

            IsControllable = true;

            foreach (var slot in Abilities) {
                if (slot.Ability != null) {
                    slot.Ability.Bind(transform);
                    slot.Ability.Reset();
                }
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        void OnDestroy() {

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        void Update() {
            if (Input.GetKey(KeyCode.Escape))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Title_Menu");
            }

            UpdateInputState();

            //UpdateAbilities();


            var moveSpeed = Attributes[ValueType.MoveSpeed];
            // Move character
            Vector3 motion = GetPlayerMotion() * moveSpeed ;

           // if (UserActivatedAbility == null || !UserActivatedAbility.Ability.AlignToCamera)
                transform.LookAt(transform.position + motion);
            //else
                ; // Align to camera

            UpdateCamera();


             
            var velocity = Rb.velocity;
            velocity.x = 0;
            velocity.z = 0;

            //if (UserActivatedAbility == null || !UserActivatedAbility.Ability.LockMovement) 
                velocity += motion;
            

            if (motion.magnitude > 0) {
                Rb.velocity = velocity;
            }

            if (Rb.velocity.magnitude > 0.0005f)
                LastActive = Time.time;

            CharacterAnimator.SetFloat("WalkSpeed", motion.magnitude / moveSpeed);

            Rb.angularVelocity = Vector3.zero;

        }


        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        void UpdateCamera() {
            if (Cursor.lockState == CursorLockMode.Locked) {

                // Update Camera
                Observer.Distance = Mathf.Clamp(Observer.Distance + Input.GetAxis("ViewZoom"), Observer.Range.x, Observer.Range.y);
                Observer.Theta = Mathf.Repeat(Observer.Theta + Input.GetAxis("ViewHorizontal"), 360);
                Observer.Phi = Mathf.Clamp(Observer.Phi + Input.GetAxis("ViewVertical"), Observer.Elevation.x, Observer.Elevation.y);
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

        protected void UpdateAbilities() {
            // Chech currently-active player-activated ability
            if (UserActivatedAbility != null) {
                var continuing = UserActivatedAbility.Ability.OnUpdate(Input.GetButton(UserActivatedAbility.TriggerName));

                if (!continuing)
                    UserActivatedAbility = null;
            }

            var deactivate = new List<ScriptedAbility>();

            // Check currently-active "system"-activated ablities (Continuous Activations)
            foreach (var ability in SystemActiveAbilities) {
                var continuing = ability.OnUpdate(false);

                if (!continuing)
                    deactivate.Add(ability);

            }

            // Remove any abilities in deactivate list from SystemActiveAbilities
            foreach (var stopped in deactivate)
                SystemActiveAbilities.Remove(stopped);


            // Check all abilities if they can and should activate
            foreach (var slot in Abilities) {
                if (slot.Ability == null) continue;
                if (slot.Ability.ContinuousActivation) { // System Activated
                    if (SystemActiveAbilities.Contains(slot.Ability)) continue; // Ability is already active

                    if (slot.CanActivate) {
                        if (slot.Ability.OnBegin()) {
                            SystemActiveAbilities.Add(slot.Ability);
                        }
                    }
                }
                else { // Player Activated
                    if (UserActivatedAbility != null) continue; // Player already has an active ability

                    if (slot.CanActivate && Input.GetButton(slot.TriggerName)) {
                        if (slot.Ability.OnBegin()) {
                            UserActivatedAbility = slot;
                        }
                    }
                }
            }
        }

        void OnTriggerEnter(Collider col) {


        }


        private void OnCollisionEnter(Collision collision) {

        }
    }
}