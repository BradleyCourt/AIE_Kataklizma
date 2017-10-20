using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;
using System;

public class FireController : MonoBehaviour
{
    
    public Transform BulletSpawn;
    private PlayerController PlayerControl;
    private EntityAttributes Stats;
    public Patrol P;
    //public Transform Target;
    public GameObject Projectile;
    public float PlayerIdletime = 3f;
    public float BulletSpeed = 3;
    public float CannonFire = 50f;
    public float MachineGun = 10f;
    public float Cooldown = 2f;
    private bool CanFire;
    private bool shooting;

    public float MachineGunROF;
    public List<ValueCollection.Value> MachineGunEffects;

    protected float MachineGunNext;

    // Use this for initialization
    void Start()
    {
        // aquire target
        // aim towards the target
        // shoot
        // gun delay
        P = GetComponent<Patrol>();
        CanFire = true;

        MachineGunNext = Time.time;
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
            PlayerControl = P.Target.GetComponent<PlayerController>(); // TODO - setTarget function
            Vector3 targetDir = P.Target.position - transform.position; 
            float step = BulletSpeed * Time.deltaTime;

            //if the player is idle longer than the allocated amount of time
            if (PlayerControl.IdleTime > PlayerIdletime)
            {
                if (CanFire)
                FireCannon();
            }
            else
            {
                // if not firing the cannon, fire the machine gun
                if(CanFire)
                FireMachineGun(targetDir);
            }


        }
    }

    void FireMachineGun(Vector3 targetDir)
    {
        // Shooting machine gun, and draweing ray in scene view
        //P.Target.GetComponent<EntityAttributes>().RemoveHealth(MachineGun * Time.deltaTime);
        //P.Target.GetComponent<EntityAttributes>().ApplyEffect(ValueType.Damage, MachineGun * Time.deltaTime);
        if (MachineGunNext <= Time.time)
        {
            MachineGunNext = Time.time + (1.0f / MachineGunROF);
            P.Target.GetComponent<EntityAttributes>().ApplyEffects(MachineGunEffects);
        }

        Debug.DrawRay(transform.position, targetDir, Color.red);
    }

    void FireCannon()
    {
        //shoot the cannon ball;
        CanFire = false;
        StartCoroutine(FireSequence());
        //StartCoroutine(this.DelayedAction(Cooldown,
        //    () => {

        //        //place target


        //        CanFire = true;

        //        GameObject go = Instantiate(Projectile, BulletSpawn.position, BulletSpawn.rotation);
        //        go.GetComponent<Rigidbody>().velocity = BulletSpawn.forward * BulletSpeed;
        //        Destroy(go, 3);

        //        return;
        //        yield return new WaitForSeconds(Cooldown);
        //    }));


        //GameObject go = Instantiate(Projectile, BulletSpawn.position, BulletSpawn.rotation);
        //go.GetComponent<Rigidbody>().v*elocity = BulletSpawn.forward * BulletSpeed;
        //Destroy(go, 3);

    }

    IEnumerator FireSequence()
    {

        yield return new WaitForSeconds(Cooldown);

        CanFire = true;

        GameObject go = Instantiate(Projectile, BulletSpawn.position, BulletSpawn.rotation);
        go.GetComponent<Rigidbody>().velocity = BulletSpawn.forward * BulletSpeed;
        Destroy(go, 3);

        yield return new WaitForSeconds(Cooldown);

        // do other stuff...
    }

}
