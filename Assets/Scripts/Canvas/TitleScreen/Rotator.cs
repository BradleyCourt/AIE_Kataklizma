using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
    public GameObject objectToRotate;
    public int rotationSpeedSide;
    public int rotationSpeedUp;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        objectToRotate.transform.Rotate(0, rotationSpeedSide * Time.deltaTime, rotationSpeedUp * Time.deltaTime);
    }
}
