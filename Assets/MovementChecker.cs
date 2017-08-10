using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementChecker : MonoBehaviour {

    int framesStatic = 0;
    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (rb.velocity.magnitude < 0.1f)
            framesStatic++;
        else
            framesStatic = 0;
	}

    public bool IsStationary()
    {
        return framesStatic > 10;
    }
}
