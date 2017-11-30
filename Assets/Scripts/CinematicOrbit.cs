using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicOrbit : MonoBehaviour {
    public Kataklizma.Gameplay.PlayerController Controller;
	// Use this for initialization
	void Start () {
        Controller.OrbitCamera = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
