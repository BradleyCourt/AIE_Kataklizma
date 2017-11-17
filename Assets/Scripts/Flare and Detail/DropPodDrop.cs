using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPodDrop : MonoBehaviour {

    CarBounce bounceScript;
    TweenRotator[] doorScript;

    public float DropTime = 1f;

    public GameObject explosionPrefab;

    [Space]
    public GameObject ContainedUnit;
    public float ReleaseHeight;

    // Use this for initialization
    void Start () {
        bounceScript = gameObject.GetComponent<CarBounce>();
        doorScript = gameObject.GetComponentsInChildren<TweenRotator>();

        LeanTween.moveLocalY(gameObject, ReleaseHeight, DropTime).setOnComplete(GroundHit);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void GroundHit()
    {
        //Instantiate(explosionPrefab, gameObject.transform);
        bounceScript.Activate();
        for (int i = 0; i < doorScript.Length; i++)
        {
            doorScript[i].RotateTo();
        }
        
    }
}
