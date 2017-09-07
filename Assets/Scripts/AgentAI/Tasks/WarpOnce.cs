using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[DisallowMultipleComponent]
public class WarpOnce : MonoBehaviour {

    public Vector3 WarpNearestTo;

    private NavMeshAgent Agent;

    // Use this for initialization
    void Start ()
    {
        Agent = GetComponent<NavMeshAgent>();
        if (Agent == null) throw new System.ApplicationException(gameObject.name + " - WarpOnce: Unable to locate required NavMeshAgent sibling");
        //tank.Warp(new Vector3(20, 0, 20));
	}
	
	// Update is called once per frame
	void Update () {

        if ( Agent.Warp(WarpNearestTo) )
        {
            enabled = false;
            Destroy(this);
        }
	}
}
