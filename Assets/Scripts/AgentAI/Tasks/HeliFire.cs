using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliFire : MonoBehaviour {

    public float LockOnTime; // helicopter 2 secs, tank 0 secs for now
    float TimeLockedOn; // dynamic data, how long we've been locked on

    Patrol P;
    Vector3 FireLocation; // the position on the ground the helicopter will fire upon
    public GameObject Rocket; // define the object of what a rocket is
    public Transform RocketPos; //position of which rockets are fired from
    public float RocketSpeed; // speed of rockets
    public GameObject TargettingCircle;

    // Use this for initialization
    void Start ()
    {
        P = GetComponent<Patrol>();
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (P.Target != null)
        {
            TimeLockedOn += Time.deltaTime;

            if (TimeLockedOn >= LockOnTime)
            {
                FireLocation = P.Target.transform.position;
                // fire at fireLocation
                LaunchAirstrike();
                TimeLockedOn = 0;
            }
            else
            {
                GameObject reticle = Instantiate(TargettingCircle);

                // draw projector/reticle and follow player

            }
        }
        else
        {
            // make the reticle fade out/invisible
            TimeLockedOn = 0;
        }
	}

    void LaunchAirstrike()
    {
        GameObject go = Instantiate(Rocket, RocketPos.position, RocketPos.rotation);
        go.GetComponent<Rigidbody>().velocity = RocketPos.forward * RocketSpeed;

        Destroy(go, 3);
    }
}
