using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenRotator : MonoBehaviour {
    public float rotation;
    public float time;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("o"))
        {
            RotateTo();
        }
	}

    public void RotateTo()
    {
       // LeanTween.rotateX(gameObject, endRotation, time).setEase(LeanTweenType.easeInExpo);
        LeanTween.rotateAroundLocal(gameObject, Vector3.right, rotation, time).setEase(LeanTweenType.easeInExpo);

    }
}
