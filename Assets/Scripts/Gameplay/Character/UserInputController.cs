using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Kataklizma.Gameplay.Character {

    [RequireComponent(typeof(CharacterManager))]
    public class UserInputController : MonoBehaviour {

        [System.Serializable]
        public class ObserverOptions {
            public float Distance = 30;
            public float ElevationOffset = 0;
            public float ElevationMin = 20;
            
            public float ElevationMax = 70;

            [HideInInspector]
            public Camera Camera;

            [HideInInspector]
            public float Theta;

            [HideInInspector]
            public float Phi;


        }

        public ObserverOptions Observer;

        protected CharacterManager Cm;

        // Use this for initialization
        void Start() {
            Observer.Camera = GetComponentInChildren<Camera>();
            if (Observer.Camera == null) throw new System.ApplicationException(gameObject.name + " - UserInputController: Unable to locate required Camera component in child objects.");

            Cm = GetComponent<CharacterManager>();
            if (Cm == null) throw new System.ApplicationException(gameObject.name + " - UserInputController: Unable to locate required CharacterManager sibling.");
        }


        // Update is called once per frame
        void Update() {
            UpdateInputState();

            // Do Move

            // Update Camera
            UpdateCamera();
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
                Observer.Phi = Mathf.Clamp(Observer.Phi + Input.GetAxis("ViewVertical"), Observer.ElevationMin, Observer.ElevationMax);
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
            Observer.Camera.transform.LookAt(transform);
            Observer.Camera.transform.Rotate(Observer.ElevationOffset, 0, 0, Space.Self);

        }

        /// <summary>
        /// Determine Camera-aligned motion
        /// </summary>
        Vector3 GetPlayerMotion() {

            var cameraDirection = transform.position - Observer.Camera.transform.position;

            var characterFwd = new Vector3(cameraDirection.x, 0, cameraDirection.z).normalized;
            var characterRight = new Vector3(cameraDirection.z, 0, -cameraDirection.x).normalized;

            var motion = characterFwd * Input.GetAxis("MoveVertical") + characterRight * Input.GetAxis("MoveHorizontal");

            if (motion.magnitude > 0.01f)
                motion.Normalize();
            else
                motion = Vector3.zero;

            return motion;
        }
    }
}