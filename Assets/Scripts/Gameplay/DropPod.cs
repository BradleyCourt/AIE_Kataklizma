using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPod : MonoBehaviour
{
	public GameObject dropPod;
	public GameObject target;
    public float height = 40;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space"))
		{
            Vector3 spawn = target.transform.position + new Vector3(0,height,0);
			Instantiate(dropPod, spawn, Quaternion.identity);
		}
	}
}
