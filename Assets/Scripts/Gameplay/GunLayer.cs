using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunLayer: MonoBehaviour {

    [System.Serializable]
    [System.Flags]
    public enum FireMode {
        DirectFire = (1 << 0), // 1
        IndirectFire = (1 << 1), // 2 (etc...)
        Both = DirectFire | IndirectFire,
    }

    public Transform Rotator;
    public Transform Elevator;

    [HideInInspector]
    public Transform Target;

    public FireMode Mode = FireMode.DirectFire;


	// Use this for initialization
	void Start () {
        Target = null;
	}
	
	// Update is called once per frame
	void Update () {
        DoRotate();
        DoElevate();
	}

    protected void DoRotate() {
        if ( Target == null ) {
            // No Target, align forward
            Rotator.localEulerAngles = Vector3.zero;
        }
    }

    protected void DoElevate() {
        if (Target == null) {
            // No Target, align forward
            Elevator.localEulerAngles = Vector3.zero;
        }
    }
}
