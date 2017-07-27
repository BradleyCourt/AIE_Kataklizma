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

            Selected.Observer.PositionTheta += Input.GetAxis("ViewHorizontal");
            Selected.Observer.PositionPhi = Mathf.Clamp(Selected.Observer.PositionPhi + Input.GetAxis("ViewVertical"), Selected.MinPitch, Selected.MaxPitch);

            //if (distance <= 1.0f)
            //    zoom = Mathf.Max(1.0f, zoom);

            //var offset = direction;
            //offset = yawDelta * offset;
            //offset = pitchDelta * offset;
            //offset = zoom * offset;

            // Rotate camera around target
            //Selected.Observer.transform.position = (Target.transform.position + offset);

        }

    }
}
