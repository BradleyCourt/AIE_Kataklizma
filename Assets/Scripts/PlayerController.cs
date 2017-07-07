using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int Speed = 5;
    public float DashLimit = 1;
    public int charge = 50; // not implemented yet, intention is once used movement is limited and you charge for X seconds or until collision with something
    public float JumpVel = 8.0F;
    public int TempDamage = 0;
    public CharacterController Cc;
    public GameObject Player;
    public bool HasControl { get; set; }
    //   private Vector3 moveDirection = Vector3.zero;

    private Vector3 moveDirection;

    public BoxCollider attackZone; // melee attack
    public CapsuleCollider fireBreath; // fire breath attack
    // Use this for initialization
    void Start()
    {
        Cc = GetComponent<CharacterController>();
        HasControl = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Charge"))
        {
            if (!HasControl)
            {
                DashLimit = 1;
            }
            HasControl = false;
        }

        if (HasControl)
        {
            moveDirection = new Vector3(Input.GetAxis("MoveHorizontal") * Speed, moveDirection.y, Input.GetAxis("MoveVertical") * Speed);

            if (Input.GetMouseButtonDown(0))
                MeleeAttack();

            if (Cc.isGrounded)
            {

                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = JumpVel;
                }
            }
            else
            {

                moveDirection += Physics.gravity * Time.deltaTime;
            }
        }
        else
        { 
            // there needs to be a check where if the player is charging, if it collides with an object it stops charging, but if it collides with an enemy, it does damage
            DashLimit -= Time.deltaTime;
            if (DashLimit >= 0)
            {
                moveDirection = new Vector3(0, 0, charge);
            }
            else
            {
                HasControl = true;
                DashLimit = 1;
            }
        }
        Cc.Move(moveDirection * Time.deltaTime);
    }

    void MeleeAttack()
    {

        var centreOffset = transform.localToWorldMatrix.MultiplyVector(attackZone.center);
        var centre = transform.position + centreOffset;
        var size = transform.localToWorldMatrix.MultiplyVector(attackZone.size);

        Collider[] targets = Physics.OverlapBox(centre, size, new Quaternion(), ~8);

        foreach (Collider target in targets)
        {
            if (target.gameObject.tag == "Player") continue;

            var playerStats = target.GetComponent<playerStats>();
            if (playerStats)
            {
                playerStats.RemoveHealth(TempDamage);
            }

        }

    }

    void FireAttack() // mainly a cut and paste from the function above, untested ( shouuuuuld work, just has to be called from the attack
    {

        var centreOffset = transform.localToWorldMatrix.MultiplyVector(attackZone.center);
        var centre = transform.position + centreOffset;
        var size = transform.localToWorldMatrix.MultiplyVector(attackZone.size);

        Collider[] targets = Physics.OverlapCapsule(centre, size, ~8);

        foreach (Collider target in targets)
        {
            if (target.gameObject.tag == "Player") continue;

            var playerStats = target.GetComponent<playerStats>();
            if (playerStats)
            {
                playerStats.RemoveHealth(TempDamage);
            }

        }

    }

}
