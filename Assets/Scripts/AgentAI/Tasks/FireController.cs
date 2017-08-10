using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;

public class FireController : MonoBehaviour
{
    private EntityStats Stats;
    public Patrol P;
    public Transform Target;
    public GameObject Projectile;
    public float speed;
    public float CannonFire = 50f;
    public float MachineGun = 10f;
    private bool shooting;

    
	// Use this for initialization
	void Start ()
    {
        // aquire target
        // aim towards the target
        // shoot
        // gun delay
        P = GetComponent<Patrol>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (P.Target == null)
        {
            // no target found
            Debug.Log("attach a transform");
        }
        else
        {
            Vector3 targetDir = Target.position - transform.position;
            float step = speed * Time.deltaTime;
            Debug.DrawRay(transform.position, targetDir, Color.red);
        }
    }
}
