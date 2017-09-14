using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRotor : MonoBehaviour
{
    public int speed = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(new Vector3(speed, 0, 0) * Time.deltaTime);
    }
}
