using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class ObserverController : MonoBehaviour {


    public GameObject Target;

    public Camera ObserverCam;

    private Vector3 Offset;

    [HideInInspector]
    public float PositionTheta = 0; // "Yaw"

    [HideInInspector]
    public float PositionPhi = 20; // "Pitch"

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        ObserverCam.transform.LookAt(Target.transform);
    }
}
