using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
    public GameObject objectToRotate;
    public float yAngle;
    public float zAngle;
    public float xAngle;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        objectToRotate.transform.Rotate(xAngle, yAngle, zAngle);
    }
}
