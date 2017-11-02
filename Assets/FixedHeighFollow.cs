using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedHeighFollow : MonoBehaviour {

    public Transform target;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        var position = target.position;
        position.y = transform.position.y;

        transform.position = position;
	}
}
