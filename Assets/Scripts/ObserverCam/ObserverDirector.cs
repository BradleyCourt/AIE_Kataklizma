using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverDirector : MonoBehaviour {

    [System.Serializable]
    public class ObserverDefinition {
        public ObserverController Observer;
        public float MinPitch = 30;
        public float MaxPitch = 80;
    }

    public GameObject Target;

    protected ObserverDefinition Selected;
    protected Rigidbody TargetRb;

    public List<ObserverDefinition> Observers;
    
	// Use this for initialization
	void Start () {
        if (Observers == null || Observers.Count < 1)
            throw new System.InvalidOperationException("ObserverDirector REQUIRES at least one Observer Definition");

        Selected = Observers[Random.Range(0, Observers.Count)];

        TargetRb = Target.GetComponent<Rigidbody>();
        
	}
	
	// Update is called once per frame
	void Update () {

        UpdateInputState();
        UpdateCamera();
        UpdateMotion();

    }

    /// <summary>
    /// Capture or Release mouse cursor
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
    /// Apply captured mouse motion to camera.  Update camera position.
    /// </summary>
    void UpdateCamera() {
        if (Cursor.lockState == CursorLockMode.Locked) {

            // Update Camera
            Selected.Observer.PositionTheta = Mathf.Repeat(Selected.Observer.PositionTheta + Input.GetAxis("ViewHorizontal"), 360);
            Selected.Observer.PositionPhi = Mathf.Clamp(Selected.Observer.PositionPhi + Input.GetAxis("ViewVertical"), Selected.MinPitch, Selected.MaxPitch);

        }


        // Apply Camera
        var yaw = Quaternion.Euler(0, Selected.Observer.PositionTheta, 0);
        var pitch = Quaternion.Euler(-Selected.Observer.PositionPhi, 0, 0);

        var offset = Vector3.forward * Selected.Observer.Distance;

        offset = pitch * offset;
        offset = yaw * offset;

        // Rotate camera around target
        Selected.Observer.transform.position = (Target.transform.position + offset);

        Debug.Log("View From: " + Selected.Observer.PositionTheta.ToString("N2") + " Theta, " + Selected.Observer.PositionPhi.ToString("N2") + " Phi.");
    }


    /// <summary>
    /// Apply captured motion to character.  Update character velocity.
    /// </summary>
    void UpdateMotion() {

        var cameraDirection = Target.transform.position - Selected.Observer.transform.position;

        var characterFwd = new Vector3(cameraDirection.x, 0, cameraDirection.z).normalized;
        var characterRight = new Vector3(cameraDirection.z, 0, -cameraDirection.x).normalized;


        var motion = characterFwd * Input.GetAxis("MoveVertical") + characterRight * Input.GetAxis("MoveHorizontal");
        motion.Normalize();

        Target.transform.LookAt(Target.transform.position + motion);

        TargetRb.velocity = motion * 5.0f;
    }
}
