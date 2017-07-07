using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int speed = 5;
    public int Charge = 50; // not implemented yet, intention is once used movement is limited and you charge for X seconds or until collision with something
    public float JumpVel = 8.0F;
    public int TempDamage = 0;
    public CharacterController cc;
    public GameObject Player;
    public bool isCharging = false;
    //   private Vector3 moveDirection = Vector3.zero;

    private Vector3 moveDirection;

    public BoxCollider attackZone;

    // Use this for initialization
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isCharging == false)
        {
             moveDirection = new Vector3(Input.GetAxis("Horizontal") * speed, moveDirection.y, Input.GetAxis("Vertical") * speed);
        }



        if (Input.GetMouseButtonDown(0))
            Attack();

        // Gravity at current does not work!  
        if (cc.isGrounded)
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

        cc.Move(moveDirection * Time.deltaTime);
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
