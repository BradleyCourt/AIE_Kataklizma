using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverDirector : MonoBehaviour {

    public GameObject Target;
    public ObserverController SelectedObserver;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        var view = new Vector3();
        view.x = Input.GetAxis("CameraHorizontal");
        view.y = Input.GetAxis("CameraVertical");
        view.z = Input.GetAxis("CameraZoom");

        var currentOffset = SelectedObserver.transform.position - Target.transform.position;

	}
}
