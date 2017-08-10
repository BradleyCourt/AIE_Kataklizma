using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;

public class FireController : MonoBehaviour
{
    
    public Transform BulletSpawn;
    private MovementChecker movementChecker;
    private EntityStats Stats;
    public Patrol P;
    public Transform Target;
    public GameObject Projectile;
    public float BulletSpeed = 3;
    public float CannonFire = 50f;
    public float MachineGun = 10f;
    public float Cooldown = 2f;
    private bool CanFire;
    private bool shooting;


    // Use this for initialization
    void Start()
    {
        // aquire target
        // aim towards the target
        // shoot
        // gun delay
        P = GetComponent<Patrol>();
        CanFire = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (P.Target == null)
        {
            // no target found
        }
        else
        {
            movementChecker = Target.GetComponent<MovementChecker>(); // TODO - setTatrget function
            Vector3 targetDir = Target.position - transform.position; 
            float step = BulletSpeed * Time.deltaTime;

            if (movementChecker.IsStationary())
            {
                if (CanFire)
                FireCannon();
            }
         
            {
                FireMachineGun(targetDir);
            }


        }
    }

    void FireMachineGun(Vector3 targetDir)
    {
        // Shooting machine gun, and draweing ray in scene view
        Target.GetComponent<EntityStats>().RemoveHealth(MachineGun * Time.deltaTime);
        Debug.DrawRay(transform.position, targetDir, Color.red);
    }

    void FireCannon()
    {
        //shoot the cannon ball;
        CanFire = false;
        StartCoroutine(this.DelayedAction(Cooldown,
            () => {
                CanFire = true;
            }));


        GameObject go = Instantiate(Projectile, BulletSpawn.position, BulletSpawn.rotation);
        go.GetComponent<Rigidbody>().velocity = BulletSpawn.forward * BulletSpeed;

        Destroy(go, 3);
    }
}
