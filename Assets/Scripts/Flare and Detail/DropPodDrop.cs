using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPodDrop : MonoBehaviour {
    CarBounce bounceScrpit;
    TweenRotator[] doorScript;
    public float time = 1f;
    public GameObject explosionPrefab;
    // Use this for initialization
    void Start () {
        bounceScrpit = gameObject.GetComponent<CarBounce>();
        doorScript = gameObject.GetComponentsInChildren<TweenRotator>();
        LeanTween.moveLocalY(gameObject, 0f, time).setOnComplete(GroundHit);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void GroundHit()
    {
        //Instantiate(explosionPrefab, gameObject.transform);
        bounceScrpit.Activate();
        for (int i = 0; i < doorScript.Length; i++)
        {
            doorScript[i].RotateTo();
        }
        
    }
}
