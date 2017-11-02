using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelect : MonoBehaviour {

    public GameObject[] cars;
	// Use this for initialization
	void Start () {
        Instantiate(cars[Random.Range(0, cars.Length)], transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
