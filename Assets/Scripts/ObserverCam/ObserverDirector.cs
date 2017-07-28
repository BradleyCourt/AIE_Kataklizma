﻿using System.Collections;
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

    [HideInInspector]
    public ObserverDefinition Selected;

    [HideInInspector]
    public Rigidbody TargetRb;

    public List<ObserverDefinition> Observers;
    
	// Use this for initialization
	void Start () {
        if (Observers == null || Observers.Count < 1)
            throw new System.InvalidOperationException("ObserverDirector REQUIRES at least one Observer Definition");

        Selected = Observers[Random.Range(0, Observers.Count)];
        Selected.Observer.Target = Target;

        TargetRb = Target.GetComponent<Rigidbody>();
        
	}
	
	// Update is called once per frame
	void Update () {

        UpdateInputState();
        UpdateCamera();

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

        //Debug.Log("View From: " + Selected.Observer.PositionTheta.ToString("N2") + " Theta, " + Selected.Observer.PositionPhi.ToString("N2") + " Phi.");
    }



}
