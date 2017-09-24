using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("space"))
        {
            //LeanTween.scale(gameObject, new Vector3(0.5f,0.5f,0.5f), 0.2f);
            //TODO: pass in direction of hit, move the gameobject slightly in that direction with a shake curve, then move it back
        }
	}
}
