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

    public List<ObserverDefinition> Observers;
    
	// Use this for initialization
	void Start () {
        if (Observers == null || Observers.Count < 1)
            throw new System.InvalidOperationException("ObserverDirector REQUIRES at least one Observer Definition");

        Selected = Observers[Random.Range(0, Observers.Count)];

        
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(2)) { // Middle Mouse toggles mouse capture
            if (Cursor.lockState != CursorLockMode.Locked) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            } else {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }


        if (Cursor.lockState == CursorLockMode.Locked) {
            var view = new Vector3();
            view.x = Input.GetAxis("ViewHorizontal");
            view.y = Input.GetAxis("ViewVertical");

            var currentOffset = Selected.Observer.transform.position - Target.transform.position;
            var distance = currentOffset.magnitude;
            var direction = currentOffset.normalized;
            var track = new Vector3(direction.x, 0, direction.z).normalized; // Direction along ground plane

            // Yaw Delta
            var yawDelta = Quaternion.Euler(new Vector3(0, Input.GetAxis("ViewHorizontal"), 0));

            // Pitch Delta (Clamped)
            var currentPitch = 90.0f - (Mathf.Asin(Vector3.Dot(direction, track)) * Mathf.Rad2Deg);
            var pitchDeltaMin = Selected.MinPitch - currentPitch;
            var pitchDeltaMax = Selected.MaxPitch - currentPitch;
            var pitchDelta = Quaternion.Euler(new Vector3(Mathf.Clamp(Input.GetAxis("ViewVertical"), pitchDeltaMin, pitchDeltaMax), 0, 0));

            Debug.Log("Current Pitch: [" 
                + Selected.MinPitch.ToString("N2") + ":" + pitchDeltaMin.ToString("N2") + "] < "
                + currentPitch.ToString("N2") + " < [" 
                + Selected.MaxPitch.ToString("N2") + ":" + pitchDeltaMax.ToString("N2") + "]");

            var xform = Matrix4x4.identity;
            

            // Zoom Delta
            var zoom = distance; // * Mathf.Clamp((1.0f + Input.GetAxis("CameraZoom")), 0.8f, 1.2f);

            //if (distance <= 1.0f)
            //    zoom = Mathf.Max(1.0f, zoom);

            var offset = direction;
            offset = yawDelta * offset;
            offset = pitchDelta * offset;
            offset = zoom * offset;

            // Rotate camera around target
            Selected.Observer.transform.position = (Target.transform.position + offset);

        }

    }
}
