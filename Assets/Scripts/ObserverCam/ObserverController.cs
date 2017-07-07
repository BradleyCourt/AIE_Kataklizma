using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class ObserverController : MonoBehaviour {


    public GameObject Target;

    public Camera ObserverCam;

    private Vector3 Offset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        //var currentDirn = ObserverCam.transform.forward;
        //var targetDirn = (Target.transform.position - transform.position).normalized;

        //var flatCurrentDirn = new Vector3(currentDirn.x, 0, currentDirn.z).normalized; // Vector component locked to XZ plane
        //var flatTargetDirn = new Vector3(targetDirn.x, 0, targetDirn.z).normalized; // Vector component locked to XZ plane

        //var yawDirn = Mathf.Asin(Vector3.Dot(flatCurrentDirn, flatTargetDirn)).CompareTo(0.0f);
        //var yawDelta = Mathf.Acos(Vector3.Dot(flatCurrentDirn, flatTargetDirn)) * Mathf.Rad2Deg * yawDirn;

        //var currentPitch = Mathf.Acos(Vector3.Dot(flatCurrentDirn, currentDirn)) * Mathf.Rad2Deg;
        //var targetPitch = Mathf.Acos(Vector3.Dot(flatTargetDirn, targetDirn)) * Mathf.Rad2Deg;

        //var pitchDelta = targetPitch - currentPitch;

        //if (Mathf.Abs(yawDelta) >= 0.1f)
        //    ObserverCam.transform.Rotate(0, yawDelta, 0, Space.World);

        //if (Mathf.Abs(pitchDelta) >= 0.1f)
        //    ObserverCam.transform.Rotate(pitchDelta, 0, 0, Space.Self);

        ObserverCam.transform.LookAt(Target.transform);

    }
}
