using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealOnTrigger : MonoBehaviour {


    public string TriggerName;

    protected MeshRenderer Mr;

	// Use this for initialization
	void Start () {
        Mr = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        Mr.enabled = Input.GetButton(TriggerName);
	}
}
