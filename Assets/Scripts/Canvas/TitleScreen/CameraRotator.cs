using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour {

	// Use this for initialization
	public GameObject objectToLookAt;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(objectToLookAt.transform);
		transform.Translate(Vector3.right * Time.deltaTime);
	}
}
