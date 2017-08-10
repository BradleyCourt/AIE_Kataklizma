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

    public Transform Traversor;
    public Transform Elevator;

    //[HideInInspector]
    public Patrol P;

    public FireMode Mode = FireMode.DirectFire;


	// Use this for initialization
	void Start () {
        //Target = null;
        P = GetComponent<Patrol>();
        if (P == null) throw new System.ApplicationException(gameObject.name + " - GunLayer: Could not locate required Patrol sibling");
	}
	
	// Update is called once per frame
	void Update () {
        if ( P.Target == null ) {
            // No Target, align forward
            Traversor.localEulerAngles = Vector3.zero;
            Elevator.localEulerAngles = Vector3.zero;
            return;
        }

        var traverseTarget = new Vector3(P.Target.position.x, Traversor.transform.position.y, P.Target.position.z);
        Traversor.LookAt(traverseTarget, Vector3.up);


    }
}
