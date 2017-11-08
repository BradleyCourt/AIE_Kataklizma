using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartDoStuf : MonoBehaviour {
    public GameObject insantiateNew;
    public bool makeNewObject = false;
    public GameObject activateOrNot;
    //public  newCoordinates;
    public bool isLooping = false;
    public float interval = 5f;
    float nextAction;
	// Use this for initialization
	void Start () {
        if(insantiateNew != null)
        {
            GameObject newThing = Instantiate(insantiateNew, transform) as GameObject;
        }
		

	}
    void Update()
        {
            if(isLooping)
            {
            if(nextAction < Time.time)
            {
                nextAction = Time.time + interval;
                if (makeNewObject)
                {
                    GameObject newThing = Instantiate(insantiateNew, transform) as GameObject;
                }
                else
                {
                    if(activateOrNot != null)
                    {
                        if(activateOrNot.activeInHierarchy)
                        {
                            activateOrNot.SetActive(false);
                        }
                        else
                        {
                            activateOrNot.SetActive(true);
                        }
                    }
                }
            }
            }
        }
}
