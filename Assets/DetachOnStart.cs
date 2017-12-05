using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachOnStart : MonoBehaviour {

    public bool LimitedLifespan = true;
    public float TimeToLive = 2;

    protected float Expiry;

	// Use this for initialization
	void Start () {
        Expiry = Time.time + TimeToLive;

        transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {

        if (Time.time > Expiry)
            Destroy(gameObject);
	}
}
