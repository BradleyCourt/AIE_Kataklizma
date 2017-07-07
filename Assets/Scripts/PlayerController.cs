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
    public bool HasControl { get; set;}
    //   private Vector3 moveDirection = Vector3.zero;

    private Vector3 moveDirection;

    public BoxCollider attackZone;

    // Use this for initialization
    void Start()
    {
        Cc = GetComponent<CharacterController>();
        HasControl = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (HasControl)
        {
             moveDirection = new Vector3(Input.GetAxis("MoveHorizontal") * Speed, moveDirection.y, Input.GetAxis("MoveVertical") * Speed);
        }

        if (Input.GetButton("Charge"))
        {
            HasControl = false;
 
            DashLimit -= Time.deltaTime;
            if (DashLimit >= 0)
            {
                moveDirection = new Vector3(0, 0, charge);
            }
            else
            {
               HasControl = true;
                Debug.Log("stoppppp");
                DashLimit = 1;
            }
        }



        if (Input.GetMouseButtonDown(0))
            Attack();

        // Gravity at current does not work!  
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

        Cc.Move(moveDirection * Time.deltaTime);
    }

    void Attack()
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

}
