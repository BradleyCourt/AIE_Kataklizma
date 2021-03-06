﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliFire : MonoBehaviour {

    public float LockOnTime; // helicopter 2 secs, tank 0 secs for now
    float TimeLockedOn; // dynamic data, how long we've been locked on

    public Patrol P;
    public GameObject Rocket; // define the object of what a rocket is
    public List<Transform> RocketOrigins; //position of which rockets are fired from
    public float RocketSpeed; // speed of rockets
    public GameObject target; // where the reticle will be placed
    public GameObject TargettingCircle;
    private bool CanFire;
    public float RocketCooldown = 0.1f;
    public int amount = 0;

    protected int _RocketPosIdx = -1;
    protected int RocketPosIdx {
        get {
            _RocketPosIdx++;

            if (_RocketPosIdx >= RocketOrigins.Count) _RocketPosIdx = 0;

            return _RocketPosIdx;
        }
    }

    protected Transform RocketPos {
        get {
            if (RocketOrigins.Count == 0) return null;

            return RocketOrigins[RocketPosIdx];
        }
    }
    // Use this for initialization
    void Start ()
    {
        P = GetComponent<Patrol>();
        CanFire = true;

    }
	
	// Update is called once per frame
	void Update ()
    {
        
        if (P.Target != null)
        {
            if (CanFire)
            {
                target = P.Target.gameObject;
                StartCoroutine(FireSequence());
            }
            //TimeLockedOn += Time.deltaTime;

            //if (TimeLockedOn >= LockOnTime)
            //{
            //    FireLocation = P.Target.transform.position;
            //    // fire at fireLocation

            //    LaunchAirstrike();
            //    TimeLockedOn = 0;
            //}
            //else
            //{

            //    GameObject reticle = Instantiate(TargettingCircle);
            //    //TargettingCircle.transform = FireLocation;    

            //    // draw projector/reticle and follow player

            //}
        }
        else
        {
            // make the reticle fade out/invisible
            TimeLockedOn = 0;
        }
	}

    void LaunchAirstrike()
    {
        //GameObject go = Instantiate(Rocket, RocketPos.position, RocketPos.rotation);
        //go.GetComponent<Rigidbody>().velocity = RocketPos.forward * RocketSpeed;
        //Destroy(go, 3);
    }

    IEnumerator FireSequence()
    {
        

        // paint the reticle
        CanFire = false;
        

        GameObject reticle = Instantiate(TargettingCircle);
        reticle.transform.position = target.transform.position;
        reticle.SetActive(true);
        yield return new WaitForSeconds(LockOnTime);



        for (int i = 0; i < amount; i++)
        {
            GameObject go = Instantiate(Rocket, RocketPos.position, RocketPos.rotation);
            go.transform.forward = reticle.transform.position - RocketPos.position;

            go.GetComponent<Rigidbody>().velocity = go.transform.forward * RocketSpeed;
            
            Destroy(go, 3);
            
            yield return new WaitForSeconds(RocketCooldown);
        }

        // destory reticle
        Destroy(reticle, 0.7f);

        yield return new WaitForSeconds(2); // Firing Sequence Cooldown

        CanFire = true;
        // do other stuff...
    }
}
